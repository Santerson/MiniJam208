using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Collider2D refWeaponCollider;
    [SerializeField] float attackUptime = 0.4f;

    /// <summary>
    /// The attack speed of the player
    /// </summary>
    public float currentAttackSpeed = 1f;
    /// <summary>
    /// The attack damage of the player's weapon
    /// </summary>
    public float currentAttackDamage = 1f;
    /// <summary>
    /// The attack range of the player's weapon
    /// </summary>
    public float currentAttackRange = 1f;

    float baseAttackSpeed;
    float baseAttackDamage;
    float baseAttackRange;

    public bool IsAttacking { get; private set; } = false;

    private void Start()
    {
        baseAttackSpeed = currentAttackSpeed;
        baseAttackDamage = currentAttackDamage;
        baseAttackRange = currentAttackRange;
    }


    /// <summary>
    /// Called when the player attacks, enables the weapon collider for a short bit
    /// </summary>
    /// <param name="context"></param>
    public void OnAttack(InputAction.CallbackContext context)
    {
        // Check if the player is attacking
        if (context.performed)
        {
            // Do attack logic
            IsAttacking = true;
            refWeaponCollider.gameObject.SetActive(true);
            // Disable the weapon collider after a short delay
            StopAllCoroutines();
            StartCoroutine(WaitDisableCollider());
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

    public void ResetStats()
    {
        currentAttackSpeed = baseAttackSpeed;
        currentAttackDamage = baseAttackDamage;
        currentAttackRange = baseAttackRange;
    }
}
