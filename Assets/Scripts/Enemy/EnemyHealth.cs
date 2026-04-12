using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] GameObject Parent;
    [SerializeField] GameObject ReferenceDamageNumber;
    [Tooltip("Spark Particle for the players attack")]
    [SerializeField] GameObject SparkPrefab;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite[] alts = new Sprite[4];
    Animator anim;
    public bool IsPurple = false;

    [Header("SOULS")]
    [SerializeField] GameObject[] souls = new GameObject[4];


    [Header("Health")]
    [Tooltip("The line renderer that covers the health bar, used to show how much health the player has left")]
    [SerializeField] LineRenderer healthBarCover;
    [Tooltip("The offset of the health bar from the player, in world units")]
    [SerializeField] Vector2 HealthBarOffset = new Vector2(0, -1f);
    [Header("Variant")]
    [Tooltip("The maximum health of the player")]
    [SerializeField] float MaxHealth = 20f;

    [Header("SFX")]
    [SerializeField] AudioSource EnemyHurtSFX;
    [SerializeField] AudioSource EnemyDeathSFX;
    [SerializeField] AudioSource EnemyDropSoulSFX;

    float initialHealthBarPosition = 0;
    float currentHealth = 0;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Initializes variables
    /// </summary>
    private void Start()
    {
        EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
        if (Parent.CompareTag("BiggerBadderEnemy").Equals(true) && gameObject.CompareTag("DMGUpEnemy").Equals(true))
        {
            sr.sprite = alts[1];
            IsPurple = true;
            
        }
        else if (Parent.CompareTag("BiggerBadderEnemy").Equals(true) && gameObject.CompareTag("RegularEmey").Equals(true))
        {
            sr.sprite = alts[0];
            IsPurple = true;
        }
        else if (Parent.CompareTag("BiggerBadderEnemy").Equals(true) && gameObject.CompareTag("SpeedUpEnemy").Equals(true))
        {
            sr.sprite = alts[2];
            IsPurple = true;
        }
        else if (Parent.CompareTag("BiggerBadderEnemy").Equals(true) && gameObject.CompareTag("BiggerRangEnemy").Equals(true))
        {
            sr.sprite = alts[3];
            IsPurple = true;
        }
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
        anim.SetBool("IsPurple", IsPurple);
        // Set the health bar to that position
        healthBarCover.transform.position = (Vector2)transform.position + HealthBarOffset;
        if (currentHealth <= 0 && Parent.CompareTag("BiggerBadderEnemy").Equals(false))
        {
            EnemyDie();
        }
        else if (currentHealth <= 0 && Parent.CompareTag("BiggerBadderEnemy").Equals(true))
        {
            int random = Random.Range(0, souls.Length);
            Instantiate(souls[random], gameObject.transform.position, Quaternion.identity);
            Instantiate(EnemyDropSoulSFX, transform.position, Quaternion.identity);
            EnemyDie();
        }
    }

    /// <summary>
    /// Adds a certain amount of health to the player and updates the health bar accordingly
    /// NOTE: subtract health by adding a negative number
    /// </summary>
    /// <param name="added">the amount of health to add / subtract</param>
    public void AddHealth(float added)
    {
        // Make it so max health cannot be exceeded
        if (currentHealth + added > MaxHealth)
        {
            added = MaxHealth - currentHealth;
        }
        // Change health
        ChangeHealth(currentHealth + added);
    }

    /// <summary>
    /// Changes the health of the player and updates the health bar accordingly
    /// </summary>
    /// <param name="newHealth">The new health of the player</param>
    public void ChangeHealth(float newHealth)
    {
        // Spawn a damage number
        GameObject damageNumber = Instantiate(ReferenceDamageNumber, transform.position, Quaternion.identity);
        damageNumber.GetComponentInChildren<DamageNumber>().damageNumberType =  currentHealth < newHealth ? DamageNumber.DamageNumberType.EnemyHeal : DamageNumber.DamageNumberType.EnemyTakeDamage;
        damageNumber.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.Abs(currentHealth - newHealth).ToString();
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
            PlayerAttack PA = GameObject.Find("PlayerObjectDontRename").GetComponent<PlayerAttack>();
            if (PA.IsAttacking)
            {
                DamageTaken(PA.GetCurrentAttackDamage());
            }
        }
    }

    private void DamageTaken(float damage)
    {
        Instantiate(SparkPrefab, transform.position, Quaternion.identity);
        Instantiate(EnemyHurtSFX, transform.position, Quaternion.identity);
        ChangeHealth(currentHealth - damage);
    }

    private void EnemyDie()
    {
        // Spawn an explosion if it should
        PlayerAttack refPlayerAttack = FindFirstObjectByType<PlayerAttack>();
        EnemySpawnSystem spawnSystem = FindFirstObjectByType<EnemySpawnSystem>();
        if (refPlayerAttack.currentEnemyDeathAOEScale > 0)
        {
            refPlayerAttack.SpawnEnemyDeathAOE(transform.position);
        }
        GameManager.Instance.EnemiesKilled++;
        spawnSystem.TotalEnemiesCurrentlySpawned--;
        
        // TODO: Death effect
        Instantiate(EnemyDeathSFX, transform.position, Quaternion.identity);

        // Destroy the enemy
        Destroy(Parent);
    }

    /// <summary>
    /// Gets the current max health
    /// </summary>
    /// <returns>float of the max health</returns>
    public float GetMaxHealth()
    {
        return MaxHealth;
    }

    /// <summary>
    /// Changes the max health of the enemy
    /// </summary>
    /// <param name="newHealth">The new health</param>
    /// <param name="fullHeal">Whether or not to fully heal the enemy</param>
    public void ChangeMaxHealth(float newHealth, bool fullHeal = true)
    {
        MaxHealth = newHealth;
        if (fullHeal)
        {
            ChangeHealth(MaxHealth);
        }
    }
}
