using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuActivation : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    bool PauseMenuActive = false;

    public void OnPauseMenuActivation(InputAction.CallbackContext context)
    {
        if (context.performed && PauseMenuActive == false)
        {

            PauseMenu.SetActive(true);
            PauseMenuActive=true;
            Time.timeScale = 0f;
        }
    }

    public void FixTimeScale()
    {
        Time.timeScale = 1f;
    }
}
