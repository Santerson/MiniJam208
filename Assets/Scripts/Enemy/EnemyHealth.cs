using NUnit.Framework;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    PlayerAttack PA;

    [Header("SOULS")]
    [SerializeField] GameObject[] souls = new GameObject[4];

    [Header("Test")]
    [SerializeField]float health = 5f;
    private void Update()
    {
        if (health <= 0 && gameObject.CompareTag("BiggerBadderEnemy").Equals(false))
        {
            Destroy(gameObject);
        }
        else if (health <= 0 && gameObject.CompareTag("BiggerBadderEnemy").Equals(true))
        {
            int random = Random.Range(0, 6);
            Instantiate(souls[random], gameObject.transform.position, Quaternion.identity);
            Debug.Log("Spawwn Soul");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D AttackTrigger)
    {
        if (AttackTrigger.gameObject.CompareTag("PlayerAttack"))
        {
            PA = AttackTrigger.gameObject.GetComponentInParent<PlayerAttack>();
            if (PA.IsAttacking)
            {
                DamageTaken(PA.currentAttackDamage);
            }
        }
    }

    private void DamageTaken(float damage)
    {
        health -= damage;
    }
}
