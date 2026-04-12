using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovemenmt : MonoBehaviour
{
    // Speed of Player
    [SerializeField] float baseMovement = 5f;
    [SerializeField] float minimumMovement = 1f;
    [SerializeField] AudioSource FootstepSFX;
    // RigidBody to move the characters
    [SerializeField] Rigidbody2D rb;
    Animator anim;
    // A Vector2 for the Movement
    Vector2 movement;
    bool movementInverted = false;
    bool isMoving = false;

    [HideInInspector] public float currentMoveSpeed { get; private set; }


    private void Awake()
    {
        // Get the rigid body
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        currentMoveSpeed = baseMovement;
    }

    // To get the context of the Value of the Inputs
    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>() * (movementInverted ? -1 : 1);
        if (movement != Vector2.zero && !isMoving)
        {
            FootstepSFX.Play();
            isMoving = true;
        }
        else if (movement == Vector2.zero && isMoving)
        {
            FootstepSFX.Stop();
            isMoving = false;
        }
    }

    private void FixedUpdate() // To contant update the Moving of the player
    {
        PlayerMoving();
        PlayerLooking();
    }

    private void PlayerMoving() // To move the Player
    {
        anim.SetFloat("movingY", movement.y);
        anim.SetFloat("movingX", movement.x);
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
        movementInverted = false;
    }

    /// <summary>
    /// Inverts the player's movement controls
    /// Resets on ResetMoveSpeed() call
    /// </summary>
    public void InvertMovement()
    {
        movementInverted = true;
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
