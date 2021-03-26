using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// Move speed of the player
    [SerializeField] float moveSpeed = 1;
    /// Jump strength of the player
    [SerializeField] float jumpStrength = 7.5f;
    /// Rigidbody component of the player
    private Rigidbody2D playerRigidBody;
    /// Animator component of the player
    private Animator playerAnimator;
    /// Sprite renderer component of the player
    private SpriteRenderer playerSpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        bool onGround = (playerRigidBody.GetContacts(new List<Collider2D>()) > 0); // Checks if the player is colliding with ANYTHING

        playerAnimator.SetFloat("MoveSpeed", Mathf.Abs(horizontalInput));
        playerAnimator.SetFloat("VerticalSpeed", playerRigidBody.velocity.y);

        if(horizontalInput != 0){
            playerRigidBody.velocity = new Vector2(horizontalInput * moveSpeed, playerRigidBody.velocity.y);

            if(horizontalInput > 0) {
                playerSpriteRenderer.flipX = false;
            } else {
                playerSpriteRenderer.flipX = true;
            }

        }
        if(Input.GetButtonDown("Jump") && onGround) {
            playerRigidBody.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
        }
    }
}
