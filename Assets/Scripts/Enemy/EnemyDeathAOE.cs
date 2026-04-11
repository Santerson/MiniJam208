using UnityEngine;

public class EnemyDeathAOE : MonoBehaviour
{
    [SerializeField] private float duration = 0.2f;

    [HideInInspector] public float scale = 1f;
    [HideInInspector] public float damageAmount = 10f;

    private void Start()
    {
        // Scale the size of the gameObject
        transform.localScale *= scale;
        // Destroy the AOE after the specified duration
        Destroy(transform.parent.gameObject, duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.AddHealth(-damageAmount);
            }
        }
    }
}
