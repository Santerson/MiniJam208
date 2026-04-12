using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Component References")]
    [Tooltip("The collider of the player's weapon")]
    [SerializeField] Collider2D refWeaponCollider;
    [Tooltip("The point from which the weapon collider will be scaled")]
    [SerializeField] GameObject refWeaponHitboxScalePoint;
    [Tooltip("The explosion aoe on enemy death")]
    [SerializeField] GameObject EnemyDeathAOEPrefab;
    [Tooltip("To show the animation of the player attacking")]
    [SerializeField] Animator anim;

    [Header("Attack Settings")]
    [Tooltip("The amount of time the weapon collider is active when the player attacks, in seconds")]
    [SerializeField] float attackUptime = 0.4f;
    [Tooltip("The base attack cooldown of the player, in seconds. The actual cooldown is 1 / attack speed")]
    [SerializeField] float baseAttackCooldown = 1f;
    [SerializeField] float AttackColliderOffset = 0.5f;
    [SerializeField] AudioSource AttackSFX;

    [Header("Base Stats")]
    [SerializeField] float baseAttackSpeed = 1;
    [SerializeField] float baseAttackDamage = 1;
    [SerializeField] float baseAttackRange = 1;
    [SerializeField] float baseSelfAttack = 0;
    [SerializeField] float baseEnemyDeathAOEScale = 0;
    [SerializeField] float baseEnemyDeathAOEDamage = 0;

    /// <summary>
    /// The attack speed of the player
    /// </summary>
    [HideInInspector] public float currentAttackSpeed = 1f;
    /// <summary>
    /// The attack damage of the player's weapon. NOTE: You should call GetCurrentAttackDamage() to get outgoing damage.
    /// </summary>
    [HideInInspector] public float currentAttackDamage = 1f;
    /// <summary>
    /// The attack range of the player's weapon
    /// </summary>
    [HideInInspector] public float currentAttackRange = 1f;
    /// <summary>
    /// The base damage multiplier of the player's weapon.
    /// </summary>
    [HideInInspector] public float currentDamageMultiplier = 1f;

    [HideInInspector] public float currentSelfAttack = 0f;
    [HideInInspector] public float currentEnemyDeathAOEScale = 0f;
    [HideInInspector] public float currentEnemyDeathAOEDamage = 0f;

    public bool IsAttacking { get; private set; } = false;

    float AttackCooldownLeft = 0f;
    Vector3 baseScale = Vector3.one;
    PlayerHealth refPlayerHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        baseAttackSpeed = currentAttackSpeed;
        baseAttackDamage = currentAttackDamage;
        baseAttackRange = currentAttackRange;
        baseSelfAttack = currentSelfAttack;
        baseScale = refWeaponHitboxScalePoint.transform.localScale;
        refPlayerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        anim.SetBool("IsAttacking", IsAttacking);
        // Reduce atk cd
        if (AttackCooldownLeft != 0)
            AttackCooldownLeft = Mathf.Max(0, AttackCooldownLeft - Time.deltaTime);
        // If attacking, move the collider to in front of the player
        refWeaponHitboxScalePoint.transform.position = transform.position + transform.up * AttackColliderOffset;

        // Check if the player is attacking
        if (Input.GetMouseButton(0) && AttackCooldownLeft <= 0 && !IsAttacking)
        {
            // Set the collider's position
            refWeaponHitboxScalePoint.transform.position = transform.position;
            refWeaponHitboxScalePoint.transform.rotation = transform.rotation;
            // Do attack logic
            IsAttacking = true;
            // Resize the collider
            refWeaponHitboxScalePoint.transform.localScale = baseScale * currentAttackRange;
            // Enable the weapon collider
            refWeaponCollider.gameObject.SetActive(true);
            // Play attack sfx
            Instantiate(AttackSFX, transform.position, Quaternion.identity);
            // Disable the weapon collider after a short delay
            StartCoroutine(WaitDisableCollider());
            // Set the attack cooldown
            AttackCooldownLeft = baseAttackCooldown / (currentAttackSpeed < 0.1f ? 0.1f : currentAttackSpeed);
            // Deal self damage
            refPlayerHealth.AddHealth(-currentSelfAttack);
        }
    }

    /// <summary>
    /// Called when the player attacks, enables the weapon collider for a short bit
    /// </summary>
    public void OnAttack(InputAction.CallbackContext context)
    {
        // Fuck this function and the stupid new input system it SUCKS
    }

    /// <summary>
    /// Disables the weapon collider after a short bit
    /// </summary>
    IEnumerator WaitDisableCollider()
    {
        yield return new WaitForSeconds(attackUptime);
        refWeaponCollider.gameObject.SetActive(false);
        IsAttacking = false;
    }

    /// <summary>
    /// Resets the player's attack stats to their base values
    /// </summary>
    public void ResetStats()
    {
        currentAttackSpeed = baseAttackSpeed;
        currentAttackDamage = baseAttackDamage;
        currentAttackRange = baseAttackRange;
        currentSelfAttack = baseSelfAttack;
        currentEnemyDeathAOEScale = baseEnemyDeathAOEScale;
        currentEnemyDeathAOEDamage = baseEnemyDeathAOEDamage;
    }

    /// <summary>
    /// Gets the current attack damage of the player's weapon, factoring in the damage multiplier. This is the value that should be used to calculate outgoing damage.
    /// </summary>
    /// <returns>float of the damage dealt</returns>
    public float GetCurrentAttackDamage()
    {
        return currentAttackDamage * currentDamageMultiplier;
    }

    public void SpawnEnemyDeathAOE(Vector2 position)
    {
        GameObject aoe = Instantiate(EnemyDeathAOEPrefab, position, Quaternion.identity);
        aoe.GetComponentInChildren<EnemyDeathAOE>().scale = currentEnemyDeathAOEScale;
        aoe.GetComponentInChildren<EnemyDeathAOE>().damageAmount = currentEnemyDeathAOEDamage;
    }
}
