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
    [HideInInspector] public TextMeshProUGUI refText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        refText = GetComponent<TextMeshProUGUI>();
        if (refText == null)
        {
            Debug.LogError("No text element given to the reftext");
        }
        float xSpeed = Random.Range(xVelocity.x, xVelocity.y) * (Random.Range(0, 2) == 1 ? -1 : 1);
        float ySpeed = Random.Range(yVelocity.x, yVelocity.y);
        refRB.AddForce(new Vector2(xSpeed, ySpeed), ForceMode2D.Impulse);
        // Start the coroutine to destroy the object after a delay
        StartCoroutine(delayDestroyObj());
    }

    IEnumerator delayDestroyObj()
    {
        yield return new WaitForSeconds(duration);
        Destroy(transform.parent.gameObject);
    }
}
