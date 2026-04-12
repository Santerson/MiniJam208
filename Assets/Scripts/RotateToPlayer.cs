using UnityEngine;

public class RotateToPlayer : MonoBehaviour
{
    [SerializeField] Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Start()
    {
        Vector2 direction = player.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}
