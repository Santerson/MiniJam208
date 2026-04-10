using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovemenmt : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] Rigidbody2D rb;
    Vector2 movement;
    Vector2 mouse;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        PlayerLooking();
    }
    private void FixedUpdate()
    {
        PlayerMoving();
    }

    private void PlayerMoving()
    {
        rb.linearVelocity = movement * speed;
    }

    private void PlayerLooking()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouseWorldPosition.z = 0f;

        Vector2 lookDirection = (Vector2)mouseWorldPosition - (Vector2)transform.position;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}
