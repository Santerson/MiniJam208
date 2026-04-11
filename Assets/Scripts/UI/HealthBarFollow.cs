using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    Transform Player;

    private void Update()
    {
        Player.transform.position =  new Vector3(Player.transform.position.x, -5f, 0);
    }
}
