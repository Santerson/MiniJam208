using UnityEngine;

public class PickupableSoul : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoulManager soulManager = collision.gameObject.GetComponent<SoulManager>();
            if (soulManager != null)
            {
                soulManager.AddSoul(gameObject);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("The player does not have a SoulManager component");
            }
        }
    }
}
