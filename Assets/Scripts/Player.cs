using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody2D playerRigidbody;

    public bool isCrouching = false;
    public bool canJump = true;

    public float jumpForce = 5f;
    public float moveSpeed = 2f;

    void Update()
    {
        playerRigidbody.linearVelocity = new Vector2(playerRigidbody.linearVelocity.x / 2, playerRigidbody.linearVelocity.y);
        Vector2 movement = Vector2.zero;
        
        if (Input.GetKeyDown(KeyCode.UpArrow) && canJump)
        {
            movement += Vector2.up * jumpForce;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement += Vector2.left * moveSpeed;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movement += Vector2.right * moveSpeed;
        }
        playerRigidbody.AddForce(movement, ForceMode2D.Impulse);
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isCrouching = true;
            transform.localScale = new Vector3(1f, 0.5f, 1f);
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.25f);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isCrouching = false;
            transform.localScale = Vector3.one;
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.25f);
        }
    }
}
