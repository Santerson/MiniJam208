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
            switch (Random.Range(1, 5))
            {
                case 1:
                    Instantiate(souls[0], transform.position, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(souls[1], transform.position, Quaternion.identity);
                    break;
                case 3:
                    Instantiate(souls[2], transform.position, Quaternion.identity);
                    break;
                case 4:
                    //Instantiate(souls[3], transform.position, Quaternion.identity);
                    break;
            }
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
