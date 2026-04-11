using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header ("Enemy Movement")]
    Transform player;
    float speed = 3f;

    [Header ("Attack Stuff")]
    [SerializeField] Collider2D refWeaponCollider;
    [SerializeField] float BaseDamage = 1f;
    [SerializeField] float BaseDamageMultiplier = 1f;
    float stoppingDistance = 3f;
    float attackRange = 4f;
    float attackTime;
    float AttackRate = 2f;

    float currentDamage;
    float currentDamageMultiplier;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        Vector2 direction = player.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    private void FixedUpdate()
    {
        // Move to Enemies
        if(player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            
            if (distance > stoppingDistance)
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            else if (distance <= attackRange && Time.time > attackTime)
            {
                Shoot();
                StartCoroutine(WaitDisableCollider());
                attackTime = Time.time + 1f / AttackRate;
            }
        }
    }

    private void Shoot()
    {
        refWeaponCollider.gameObject.SetActive(true);
    }

    IEnumerator WaitDisableCollider()
    {
        yield return new WaitForSeconds(AttackRate);
        refWeaponCollider.gameObject.SetActive(false);

    }

    /// <summary>
    /// Changes the damage by the given amount
    /// </summary>
    /// <param name="change">float of the damage change</param>
    /// <returns></returns>
    public void ChangeDamage(float change)
    {
        currentDamage += change;
        currentDamage = Mathf.Max(0, currentDamage);
    }

    /// <summary>
    /// Changes the damage multiplier by the given amount
    /// Note: do a negative number to decrease the multiplier
    /// </summary>
    /// <param name="change">The amount to change it by</param>
    public void ChangeDamageMultiplier(float change)
    {
        currentDamageMultiplier += change;
    }

    /// <summary>
    /// Resets the enemy damage and multiplier to their base values
    /// </summary>
    public void ResetDamage()
    {
        currentDamage = BaseDamage;
        currentDamageMultiplier = BaseDamageMultiplier;
    }

    /// <summary>
    /// Gets the current damage of the enemy
    /// </summary>
    /// <returns>float of the damage</returns>
    public float getDamage()
    {
        return currentDamageMultiplier * currentDamage;
    }
}
