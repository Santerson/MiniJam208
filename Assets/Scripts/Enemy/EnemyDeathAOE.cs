using UnityEngine;

public class EnemyDeathAOE : MonoBehaviour
{
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private ParticleSystem explosionEfx;
    [SerializeField] private ParticleSystem healEFX;

    [HideInInspector] public float scale = 1f;
    [HideInInspector] public float damageAmount = 10f;

    private void Start()
    {
        // Scale the size of the gameObject
        transform.localScale *= scale;
        // Destroy the AOE after the specified duration
        Destroy(transform.parent.gameObject, duration);
        if (damageAmount > 0)
        {
            Instantiate(explosionEfx, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(healEFX, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.CompareTag("Enemy") || other.transform.parent.CompareTag("BiggerBadderEnemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.AddHealth(-damageAmount);
            }
        }
    }
}
