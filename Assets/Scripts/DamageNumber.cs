using System.Collections;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] float duration = 2f;
    [Tooltip("Where the number must be positive, however can also apply a negative velocity inately. It will be anywhere between the x value of the vec2 and the y value.")]
    [SerializeField] Vector2 xVelocity;
    [Tooltip("Where the number must be positive, however can also apply a negative velocity inately. It will be anywhere between the x value of the vec2 and the y value.")]
    [SerializeField] Vector2 yVelocity;
    [SerializeField] Rigidbody2D refRB;

    [Header("Colors")]
    [SerializeField] Color EnemyTakeDamageColor;
    [SerializeField] Color EnemyHealColor;
    [SerializeField] Color PlayerTakeDamageColor;
    [SerializeField] Color PlayerHealColor;

    // Access these two fields from the script that spawns the damage number
    [HideInInspector] public DamageNumberType damageNumberType;
    [HideInInspector] public TextMeshProUGUI refText;

    public enum DamageNumberType
    {
        EnemyTakeDamage,
        EnemyHeal,
        PlayerTakeDamage,
        PlayerHeal
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the text component
        refText = GetComponent<TextMeshProUGUI>();
        if (refText == null)
        {
            Debug.LogError("No text element given to the reftext");
        }
        
        // Apply a random velocity to the text
        float xSpeed = Random.Range(xVelocity.x, xVelocity.y) * (Random.Range(0, 2) == 1 ? -1 : 1);
        float ySpeed = Random.Range(yVelocity.x, yVelocity.y);
        refRB.AddForce(new Vector2(xSpeed, ySpeed), ForceMode2D.Impulse);

        // Set the color of the text
        switch (damageNumberType)
        {
            case DamageNumberType.EnemyTakeDamage:
                refText.color = EnemyTakeDamageColor;
                break;
            case DamageNumberType.EnemyHeal:
                refText.color = EnemyHealColor;
                break;
            case DamageNumberType.PlayerTakeDamage:
                refText.color = PlayerTakeDamageColor;
                break;
            case DamageNumberType.PlayerHeal:
                refText.color = PlayerHealColor;
                break;
        }
        
        // Start the coroutine to destroy the object after a delay
        StartCoroutine(DelayDestroyObj());
    }

    /// <summary>
    /// Destroys the object after a delay
    /// </summary>
    IEnumerator DelayDestroyObj()
    {
        yield return new WaitForSeconds(duration);
        Destroy(transform.parent.gameObject);
    }
}
