using UnityEngine;

public class PickupableSoul : MonoBehaviour
{
    [SerializeField] AudioSource pickupsfx;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoulManager soulManager = collision.gameObject.GetComponent<SoulManager>();
            if (soulManager != null)
            {
                soulManager.AddSoul(gameObject);
                Instantiate(pickupsfx, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("The player does not have a SoulManager component");
            }
        }
    }
}
