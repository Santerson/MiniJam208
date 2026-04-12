using UnityEngine;

public class PlayUIClick : MonoBehaviour
{
    public void PlayClickSFX()
    {
        GameObject.Find("UIclick").GetComponent<AudioSource>().Play();
    }
}
