using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary> Move speed of the player </summary>
    [SerializeField] private float moveSpeed = 1;
    /// <summary> Jump strength of the player </summary>
    [SerializeField] private float jumpStrength = 7.5f;
    /// <summary> Layer for touchable blocks </summary>
    [SerializeField] private LayerMask groundLayer;
    /// <summary> Rigidbody component of the player </summary>
    private Rigidbody2D playerRigidBody;
    /// <summary> Hitbox of the player </summary>
    private BoxCollider2D playerHitbox;
    /// <summary> Animator component of the player </summary>
    private Animator playerAnimator;
    /// <summary> Sprite renderer component of the player </summary>
    private SpriteRenderer playerSpriteRenderer;

    private void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerHitbox = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

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
        if (Input.GetButtonDown("Jump") && onGround()) {
            playerRigidBody.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
        }
    }

    private bool onGround()
    {
        /// Holds the center location of the ground detecting box, the center bottom of the hitbox
        Vector2 groundBoxCenter = playerHitbox.bounds.center;
        groundBoxCenter.y -= playerHitbox.bounds.extents.y;

        Vector2 groundBoxSize = new Vector2(playerHitbox.bounds.size.x - 0.1f, 0.1f);

        /// Check if the area directly under the player's hitbox is occupied by something in the groundlayer. Return true if yes, false if no.
        return Physics2D.OverlapBox(groundBoxCenter, groundBoxSize, 0f, groundLayer) != null;
    }
}
