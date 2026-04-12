using UnityEngine;
using TMPro;
public class DeadScene : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI TimePlayed;
    [SerializeField] TextMeshProUGUI EnemiesKilled;
    [SerializeField] TextMeshProUGUI SoulsCollected;
    [SerializeField] TextMeshProUGUI SoulsCorrupted;

    private void Update()
    {
        TimePlayed.text = "Time Played: " + (int)GameManager.Instance.PlayTime + " Seconds";
        EnemiesKilled.text = "Enemies Killed: " + (int)GameManager.Instance.EnemiesKilled;
        SoulsCollected.text = "Souls Collected: " + (int)GameManager.Instance.SoulsCollected;
        SoulsCorrupted.text = "Souls Corrupted: " + (int)GameManager.Instance.SoulsCorrupted;
    }
}
