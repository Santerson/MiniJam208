using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    PlayerAttack PA;
    [SerializeField]float health = 5f;
    private void Update()
    {
        if (health <= 0 && gameObject.CompareTag("BiggerBadderEnemy").Equals(false))
        {
            Destroy(this);
        }
        else if (health <= 0 && gameObject.CompareTag("BiggerBadderEnemy").Equals(true))
        {
            Debug.Log("Spawwn Soul");
            Destroy(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D AttackTrigger)
    {
        if (AttackTrigger.gameObject.CompareTag("PlayerAttack"))
        {
            PA = AttackTrigger.gameObject.GetComponent<PlayerAttack>();
            if (PA.IsAttacking)
            {
                DamageTaken();
            }
        }
    }

    private void DamageTaken()
    {
        health -= PA.currentAttackDamage;
    }
}
