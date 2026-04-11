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
    float stoppingDistance = 3f;
    float attackRange = 4f;
    float attackTime;
    float AttackRate = 2f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
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
}
