using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary> Move speed of the player </summary>
    [SerializeField] private float moveSpeed = 1;
    /// <summary> Jump strength of the player </summary>
    [SerializeField] private float jumpStrength = 7.5f;
    /// <summary> Layer for touchable blocks </summary>
    [SerializeField] private LayerMask groundLayer;
    /// <summary> The player's species name, used to determine sprites </summary>
    /// <remarks> Sprites should be formatted like: <c>player_[action]_[species]</c> (e.g. <c>player_walk_lizard</c>).</remarks>
    [SerializeField] private string species = "human";
    /// <summary> Rigidbody component of the player </summary>
    private Rigidbody2D playerRigidBody;
    /// <summary> Hitbox of the player </summary>
    private BoxCollider2D playerHitbox;
    /// <summary> Animator component of the player </summary>
    private SpriteAnimator playerAnimator;

    /// <summary> The spritesheet used when the player is moving </summary>
    private SpriteSheet moveSheet;
    /// <summary> The spritesheet used when the player is standing still </summary>
    private SpriteSheet idleSheet;

    private void Start()
    {
        playerAnimator = GetComponent<SpriteAnimator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerHitbox = GetComponent<BoxCollider2D>();
        moveSheet = new SpriteSheet("Sprites/player_walk_" + species);
        idleSheet = new SpriteSheet("Sprites/player_" + species);
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        playerAnimator.SetAnimation(Mathf.Abs((horizontalInput)) > 0 ? moveSheet : idleSheet);

        if (horizontalInput != 0) {
            playerRigidBody.velocity = new Vector2(horizontalInput * moveSpeed, playerRigidBody.velocity.y);

            gameObject.transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);
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
        return Physics2D.OverlapBox(groundBoxCenter, groundBoxSize, 0f, groundLayer) is { };
    }

/*
    private bool OnCeiling()        // Bonk
    {
        // Holds the center location of the ceiling detecting box, the center top of the hitbox
        Bounds bounds = playerHitbox.bounds;
        Vector2 groundBoxCenter = bounds.center;
        groundBoxCenter.y += bounds.extents.y;

        Vector2 groundBoxSize = new Vector2(bounds.size.x - 0.1f, 0.1f);

        // Check if the area directly over the player's hitbox is occupied by something in the ground layer. Return true if yes, false if no.
        return Physics2D.OverlapBox(groundBoxCenter, groundBoxSize, 0f, groundLayer) is { };
    }
*/

    private bool OnWallRight()
    {
        // Holds the center location of the right wall detecting box, the center right of the hitbox
        Bounds bounds = playerHitbox.bounds;
        Vector2 wallBoxCenter = bounds.center;
        wallBoxCenter.x += bounds.extents.x;

        Vector2 wallBoxSize = new Vector2(0.1f , bounds.size.y - 0.1f);

        // Check if the area directly right of the player's hitbox is occupied by something in the ground layer. Return true if yes, false if no.
        return Physics2D.OverlapBox(wallBoxCenter, wallBoxSize, 0f, groundLayer) is { };
    }

    private bool OnWallLeft()
    {
        // Holds the center location of the left wall detecting box, the center bottom of the hitbox
        Bounds bounds = playerHitbox.bounds;
        Vector2 wallBoxCenter = bounds.center;
        wallBoxCenter.x -= bounds.extents.x;

        Vector2 wallBoxSize = new Vector2(0.1f , bounds.size.y - 0.1f);

        // Check if the area directly left of the player's hitbox is occupied by something in the ground layer. Return true if yes, false if no.
        return Physics2D.OverlapBox(wallBoxCenter, wallBoxSize, 0f, groundLayer) is { };
    }
}
