using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    Transform player;
    Camera mainCam;

    private Vector3 currentVelocity = Vector3.zero;

    [Header("Settings")]
    [Range(0f, 1f)] public float mouseInfluence = 0.25f;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3 (0f, 0f, -10f);

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        if (player ==  null) { return; }

        Vector3 mousePos = mainCam.ScreenToWorldPoint (Input.mousePosition);
        mousePos.z = 0;

        Vector3 targetPos = Vector3.Lerp(player.position, mousePos, mouseInfluence);
        targetPos.z = -10f;

        Vector3 desiredPosition = targetPos + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);
    }
}
