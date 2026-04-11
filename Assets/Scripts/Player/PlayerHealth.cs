using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    Slider healthSlider;

    [SerializeField] float Playerhealth = 20f;

    private void Awake()
    {
        healthSlider = GameObject.FindGameObjectWithTag("HealthSlider").GetComponent<Slider>();
    }

    private void Update()
    {
        healthSlider.value = Playerhealth;
    }
}
