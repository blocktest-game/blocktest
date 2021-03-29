using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary> Move speed of the player </summary>
    [SerializeField] private float moveSpeed = 1;
    /// <summary> Jump strength of the player </summary>
    [SerializeField] private float jumpStrength = 7.5f;
    /// <summary> Rigidbody component of the player </summary>
    private Rigidbody2D playerRigidBody;
    /// <summary> Animator component of the player </summary>
    private Animator playerAnimator;
    /// <summary> Sprite renderer component of the player </summary>
    private SpriteRenderer playerSpriteRenderer;

    private void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        bool onGround = playerRigidBody.GetContacts(new List<Collider2D>()) > 0; // Checks if the player is colliding with ANYTHING

        playerAnimator.SetFloat("MoveSpeed", Mathf.Abs(horizontalInput));
        playerAnimator.SetFloat("VerticalSpeed", playerRigidBody.velocity.y);

        if (horizontalInput != 0) {
            playerRigidBody.velocity = new Vector2(horizontalInput * moveSpeed, playerRigidBody.velocity.y);

            if (horizontalInput > 0) {
                playerSpriteRenderer.flipX = false;
            } else {
                playerSpriteRenderer.flipX = true;
            }

        }
        if (Input.GetButtonDown("Jump") && onGround) {
            playerRigidBody.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
        }
    }
}
