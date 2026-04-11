using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    PlayerAttack PA;
    float health = 5f;
    private void Update()
    {
        if (health <= 0)
        {
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
