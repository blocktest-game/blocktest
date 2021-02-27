using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// Move speed of the player
    [SerializeField] float moveSpeed = 1;
    /// Jump strength of the player
    [SerializeField] float jumpStrength = 7.5f;
    [SerializeField] LayerMask groundLayer;
    /// Rigidbody component of the player
    private Rigidbody2D playerRB;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if(horizontalInput != 0) 
        {
            transform.Translate(Vector2.right * horizontalInput * moveSpeed * Time.deltaTime);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerRB.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
        }
    }
}
