using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Collider2D refWeaponCollider;
    [SerializeField] float attackUptime = 0.4f;

    public bool isAttacking { get; private set; } = false;

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
            isAttacking = true;
            refWeaponCollider.gameObject.SetActive(true);
            // Disable the weapon collider after a short delay
            StartCoroutine(waitDisableCollider());
        }
    }

    /// <summary>
    /// Disables the weapon collider after a short bit
    /// </summary>
    IEnumerator waitDisableCollider()
    {
        yield return new WaitForSeconds(attackUptime);
        refWeaponCollider.gameObject.SetActive(false);
        isAttacking = false;
    }
}
