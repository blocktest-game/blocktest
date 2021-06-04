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
    /// <summary> These are faster than using the string every time </summary>
    private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
    private static readonly int VerticalSpeed = Animator.StringToHash("VerticalSpeed");

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

        playerAnimator.SetFloat(MoveSpeed, Mathf.Abs(horizontalInput));
        playerAnimator.SetFloat(VerticalSpeed, playerRigidBody.velocity.y);

        if (horizontalInput != 0) {
            playerRigidBody.velocity = new Vector2(horizontalInput * moveSpeed, playerRigidBody.velocity.y);

            playerSpriteRenderer.flipX = !(horizontalInput > 0);

        }
        if (Input.GetButtonDown("Jump")) {
            TryJump();
        }
    }

    private void TryJump()
    {
        if (OnGround()) {
            GroundJump();
        }

        else if (OnWallRight()) {
            GroundJump();
        }

        else if (OnWallLeft()) {
            GroundJump();
        }
    }

    private void GroundJump()       // Meant to be used for jumping while on the ground
    {
        playerRigidBody.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
    }

    private bool OnGround()
    {
        // Holds the center location of the ground detecting box, the center bottom of the hitbox
        Bounds bounds = playerHitbox.bounds;
        Vector2 groundBoxCenter = bounds.center;
        groundBoxCenter.y -= bounds.extents.y;

        Vector2 groundBoxSize = new Vector2(bounds.size.x - 0.1f, 0.1f);

        // Check if the area directly under the player's hitbox is occupied by something in the ground layer. Return true if yes, false if no.
        return Physics2D.OverlapBox(groundBoxCenter, groundBoxSize, 0f, groundLayer) != null;
    }

    private bool OnCeiling()        // Bonk
    {
        // Holds the center location of the ceiling detecting box, the center top of the hitbox
        Bounds bounds = playerHitbox.bounds;
        Vector2 groundBoxCenter = bounds.center;
        groundBoxCenter.y += bounds.extents.y;

        Vector2 groundBoxSize = new Vector2(bounds.size.x - 0.1f, 0.1f);

        // Check if the area directly over the player's hitbox is occupied by something in the groundlayer. Return true if yes, false if no.
        return Physics2D.OverlapBox(groundBoxCenter, groundBoxSize, 0f, groundLayer) != null;
    }

    private bool OnWallRight()
    {
        // Holds the center location of the right wall detecting box, the center right of the hitbox
        Bounds bounds = playerHitbox.bounds;
        Vector2 wallBoxCenter = bounds.center;
        wallBoxCenter.x += bounds.extents.x;

        Vector2 wallBoxSize = new Vector2(0.1f , bounds.size.y - 0.1f);

        // Check if the area directly right of the player's hitbox is occupied by something in the groundlayer. Return true if yes, false if no.
        return Physics2D.OverlapBox(wallBoxCenter, wallBoxSize, 0f, groundLayer) != null;
    }

    private bool OnWallLeft()
    {
        // Holds the center location of the left wall detecting box, the center bottom of the hitbox
        Bounds bounds = playerHitbox.bounds;
        Vector2 wallBoxCenter = bounds.center;
        wallBoxCenter.x -= bounds.extents.x;

        Vector2 wallBoxSize = new Vector2(0.1f , bounds.size.y - 0.1f);

        // Check if the area directly left of the player's hitbox is occupied by something in the groundlayer. Return true if yes, false if no.
        return Physics2D.OverlapBox(wallBoxCenter, wallBoxSize, 0f, groundLayer) != null;
    }
}
