using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuActivation : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;

    public void OnPauseMenuActivation(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

            PauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void FixTimeScale()
    {
        Time.timeScale = 1f;
    }
}
