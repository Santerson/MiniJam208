using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [Tooltip("The collider of the player's weapon")]
    [SerializeField] Collider2D refWeaponCollider;
    [Tooltip("The point from which the weapon collider will be scaled")]
    [SerializeField] GameObject refWeaponHitboxScalePoint;
    [Tooltip("The amount of time the weapon collider is active when the player attacks, in seconds")]
    [SerializeField] float attackUptime = 0.4f;
    [Tooltip("The base attack cooldown of the player, in seconds. The actual cooldown is 1 / attack speed")]
    [SerializeField] float baseAttackCooldown = 1f;

    [Header("Base Stats")]
    [SerializeField] float baseAttackSpeed = 1;
    [SerializeField] float baseAttackDamage = 1;
    [SerializeField] float baseAttackRange = 1;
    [SerializeField] float baseSelfAttack = 0;

    /// <summary>
    /// The attack speed of the player
    /// </summary>
    [HideInInspector] public float currentAttackSpeed = 1f;
    /// <summary>
    /// The attack damage of the player's weapon
    /// </summary>
    [HideInInspector] public float currentAttackDamage = 1f;
    /// <summary>
    /// The attack range of the player's weapon
    /// </summary>
    [HideInInspector] public float currentAttackRange = 1f;

    [HideInInspector] public float currentSelfAttack = 0f;

    public bool IsAttacking { get; private set; } = false;

    float AttackCooldownLeft = 0f;
    Vector3 baseScale = Vector3.one;

    private void Start()
    {
        baseAttackSpeed = currentAttackSpeed;
        baseAttackDamage = currentAttackDamage;
        baseAttackRange = currentAttackRange;
        baseSelfAttack = currentSelfAttack;
        baseScale = refWeaponHitboxScalePoint.transform.localScale;
    }

    private void Update()
    {
        if (AttackCooldownLeft != 0)
            AttackCooldownLeft = Mathf.Max(0, AttackCooldownLeft - Time.deltaTime);
    }

    /// <summary>
    /// Called when the player attacks, enables the weapon collider for a short bit
    /// </summary>
    public void OnAttack(InputAction.CallbackContext context)
    {
        // Check if the player is attacking
        if (context.performed && AttackCooldownLeft <= 0 && !IsAttacking)
        {
            // Do attack logic
            IsAttacking = true;
            // Resize the collider
            refWeaponHitboxScalePoint.transform.localScale = baseScale * currentAttackRange;
            // Enable the weapon collider
            refWeaponCollider.gameObject.SetActive(true);

            Debug.Log($"{currentAttackDamage}dmg, {currentAttackSpeed}asp, {currentAttackRange}rng");
            // Disable the weapon collider after a short delay
            StartCoroutine(WaitDisableCollider());
            // Set the attack cooldown
            AttackCooldownLeft = baseAttackCooldown / currentAttackSpeed;
        }
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
    }
}
