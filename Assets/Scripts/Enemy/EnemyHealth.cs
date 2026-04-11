using NUnit.Framework;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    PlayerAttack PA;

    [Header("SOULS")]
    [SerializeField] GameObject[] souls = new GameObject[4];


    [Header("Health")]
    [Tooltip("The line renderer that covers the health bar, used to show how much health the player has left")]
    [SerializeField] LineRenderer healthBarCover;
    [Tooltip("The offset of the health bar from the player, in world units")]
    [SerializeField] Vector2 HealthBarOffset = new Vector2(0, -1f);
    [Tooltip("The maximum health of the player")]
    [SerializeField] float MaxHealth = 20f;

    float initialHealthBarPosition = 0;
    float currentHealth = 0;

    /// <summary>
    /// Initializes variables
    /// </summary>
    private void Start()
    {
        // Set the health bar to the correct position
        initialHealthBarPosition = healthBarCover.GetPosition(0).x;
        healthBarCover.SetPosition(1, healthBarCover.GetPosition(0));
        ChangeHealth(MaxHealth);
    }

    /// <summary>
    /// Moves the player health bar
    /// </summary>
    private void Update()
    {
        // Set the health bar to that position
        healthBarCover.transform.position = (Vector2)transform.position + HealthBarOffset;
        if (currentHealth <= 0 && gameObject.CompareTag("BiggerBadderEnemy").Equals(false))
        {
            Destroy(gameObject);
        }
        else if (currentHealth <= 0 && gameObject.CompareTag("BiggerBadderEnemy").Equals(true))
        {
            int random = Random.Range(0, souls.Length);
            Instantiate(souls[random], gameObject.transform.position, Quaternion.identity);
            Debug.Log("Spawwn Soul");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Adds a certain amount of health to the player and updates the health bar accordingly
    /// NOTE: subtract health by adding a negative number
    /// </summary>
    /// <param name="added">the amount of health to add / subtract</param>
    public void AddHealth(float added)
    {
        ChangeHealth(currentHealth + added);
    }

    /// <summary>
    /// Changes the health of the player and updates the health bar accordingly
    /// </summary>
    /// <param name="newHealth">The new health of the player</param>
    public void ChangeHealth(float newHealth)
    {
        // Change the health of the player and update the health bar
        currentHealth = newHealth;
        // Set the pos of the barcover
        float xPos = Mathf.Lerp(0, initialHealthBarPosition, currentHealth / MaxHealth);
        healthBarCover.SetPosition(1, new Vector3(xPos, healthBarCover.GetPosition(1).y, healthBarCover.GetPosition(1).z));
    }

    private void OnTriggerEnter2D(Collider2D AttackTrigger)
    {
        if (AttackTrigger.gameObject.CompareTag("PlayerAttack"))
        {
            PA = AttackTrigger.gameObject.GetComponentInParent<PlayerAttack>();
            if (PA.IsAttacking)
            {
                DamageTaken(PA.GetCurrentAttackDamage());
            }
        }
    }

    private void DamageTaken(float damage)
    {
        ChangeHealth(currentHealth - damage);
    }
}
