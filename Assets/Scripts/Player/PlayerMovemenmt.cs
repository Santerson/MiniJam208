using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovemenmt : MonoBehaviour
{
    // Speed of Player
    [SerializeField] float speed = 5f;
    // RigidBody to move the characters
    [SerializeField] Rigidbody2D rb;
    // A Vector2 for the Movement
    Vector2 movement;


    private void Awake()
    {
        // Get the rigid body
        rb = GetComponent<Rigidbody2D>();
    }

    // To get the context of the Value of the Inputs
    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    private void FixedUpdate() // To contant update the Moving of the player
    {
        PlayerMoving();
        PlayerLooking();
    }

    private void PlayerMoving() // To move the Player
    {
        rb.linearVelocity = movement * speed;
    }

    private void PlayerLooking() // To get the player to look at mouse 
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouseWorldPosition.z = 0f;

        Vector2 lookDirection = (Vector2)mouseWorldPosition - (Vector2)transform.position;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}
