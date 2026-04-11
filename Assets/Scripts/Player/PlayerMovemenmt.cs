using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovemenmt : MonoBehaviour
{
    // Speed of Player
    [SerializeField] float baseMovement = 5f;
    [SerializeField] float minimumMovement = 1f;
    // RigidBody to move the characters
    [SerializeField] Rigidbody2D rb;
    // A Vector2 for the Movement
    Vector2 movement;

    [HideInInspector] public float currentMoveSpeed { get; private set; }


    private void Awake()
    {
        // Get the rigid body
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentMoveSpeed = baseMovement;
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
        // Move the player by either the current move speed or the minimum move speed, whichever is higher
        rb.linearVelocity = movement * Mathf.Max(currentMoveSpeed, minimumMovement);
    }

    /// <summary>
    /// Changes the current speed by the given amount
    /// (Note: use negative to deduct movepseed)
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeMoveSpeed(float amount)
    {
        currentMoveSpeed += amount;
    }

    /// <summary>
    /// Resets the player's movespeed to the default amount
    /// </summary>
    public void ResetMoveSpeed()
    {
        currentMoveSpeed = baseMovement;
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
