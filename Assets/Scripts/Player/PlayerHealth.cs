using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("The line renderer that covers the health bar, used to show how much health the player has left")]
    [SerializeField] LineRenderer healthBarCover;
    [Tooltip("The offset of the health bar from the player, in world units")]
    [SerializeField] Vector2 HealthBarOffset = new Vector2(0, -1f);
    [Tooltip("The prefab for the damage number that appears when the player takes damage or heals")]
    [SerializeField] GameObject ReferenceDamageNumber;
    [Tooltip("The maximum health of the player")]
    [SerializeField] float MaxHealth = 20f;
    [SerializeField] float IFrames = 0.5f;
    [SerializeField] float BaseIncomingDmgMultiplier = 1f;
    [SerializeField] float BaseHealthRegen = 0f;
    

    float initialHealthBarPosition = 0;
    float currentHealth = 0;
    float currentIncomingDmgMultiplier = 1;
    float currentHealthRegen = 0f;
    float IframeTimeLeft = 0f;

    float timeToNextHeal = 0f;

    /// <summary>
    /// Initializes variables
    /// </summary>
    private void Start()
    {
        // Set the health bar to the correct position
        initialHealthBarPosition = healthBarCover.GetPosition(0).x;
        healthBarCover.SetPosition(1, healthBarCover.GetPosition(0));
        ChangeHealth(MaxHealth);
        currentIncomingDmgMultiplier = BaseIncomingDmgMultiplier;
        currentHealthRegen = BaseHealthRegen;
    }

    /// <summary>
    /// Moves the player health bar
    /// </summary>
    private void Update()
    {
        // Set the health bar to that position
        healthBarCover.transform.position = (Vector2)transform.position + HealthBarOffset;
        // Apply Heal over Time or Damage over Time
        if (currentHealthRegen != 0)
        {
            timeToNextHeal -= Time.deltaTime;
            if (timeToNextHeal <= 0)
            {
                AddHealth(currentHealthRegen);
                if (currentHealth > MaxHealth) currentHealth = MaxHealth;
                timeToNextHeal = 1;
            }
        }
        IframeTimeLeft = Mathf.Max(0, IframeTimeLeft - Time.deltaTime);
    }

    /// <summary>
    /// Adds a certain amount of health to the player and updates the health bar accordingly
    /// NOTE: subtract health by adding a negative number
    /// </summary>
    /// <param name="added">the amount of health to add / subtract</param>
    public void AddHealth(float added)
    {
        if (added == 0) return;
        // Spawn a damage number
        GameObject damageNumber = Instantiate(ReferenceDamageNumber, transform.position, Quaternion.identity);
        damageNumber.GetComponentInChildren<DamageNumber>().damageNumberType = added > 0 ? DamageNumber.DamageNumberType.PlayerHeal : DamageNumber.DamageNumberType.PlayerTakeDamage;
        // Set the text of the damage number to the amount of health changed
        if (added % 1 != 0)
        {
            damageNumber.GetComponentInChildren<TextMeshProUGUI>().text = $"{Mathf.Abs(added): 0.00}";
        }
        else
        {
            damageNumber.GetComponentInChildren<TextMeshProUGUI>().text = $"{Mathf.Abs(added)}";
        }
        if (added <= 0)
        {
            ChangeHealth(currentHealth + added * currentIncomingDmgMultiplier);
        }
        else
        {
            ChangeHealth(currentHealth + added > MaxHealth ? MaxHealth : currentHealth + added);
        }
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
        // Check if player ded
        if (currentHealth <= 0)
        {
            Skissue();
        }
    }

    /// <summary>
    /// Deals damage to the player if they hit an enemy attack collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyAttack") && IframeTimeLeft <= 0)
        {
            // Get the damage of the attack and subtract it from the player's health
            EnemyMovement refEnemy = collision.gameObject.GetComponentInParent<EnemyMovement>();
            if (refEnemy != null)
            {
                AddHealth(-refEnemy.getDamage());
                IframeTimeLeft = IFrames;
            }
            else
            {
                Debug.LogError("Enemy attack collider does not have an EnemyMovement script attached to its parent");
            }
        }
    }

    /// <summary>
    /// Player loses lol what a loser
    /// </summary>
    void Skissue()
    {
        SceneManager.LoadScene("Dead");
        Destroy(gameObject);
    }

    /// <summary>
    /// Changes the incoming damage multiplier by the number given
    /// NOTE: a positive number will AMPLIFY damage, a negative number will REDUCE damage
    /// </summary>
    /// <param name="change">The amount of damage changed</param>
    public void ChangeDamageMultiplier(float change)
    {
        currentIncomingDmgMultiplier += change;
        if (currentIncomingDmgMultiplier < 0) currentIncomingDmgMultiplier = 0;
    }
   
    /// <summary>
    /// Changes the health regeneration by the number given
    /// NOTE: a negative number will cause Damage over Time, a Positive Number will grant a Heal over Time
    /// NOTE: This increments in 1 unit per second
    /// </summary>
    /// <param name="change"></param>
    public void ChangeHealthRegneration(float change)
    {
        currentHealthRegen += change;
    }

    /// <summary>
    /// Resets health related stats EXCEPT current health
    /// </summary>
    public void ResetStats()
    {
        currentIncomingDmgMultiplier = BaseIncomingDmgMultiplier;
        currentHealthRegen = BaseHealthRegen;
    }
}

