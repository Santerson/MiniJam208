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
        TimePlayed.text = "Time Played: " + GameManager.Instance.PlayTime;
        EnemiesKilled.text = "Enemies Killed: " + GameManager.Instance.EnemiesKilled;
        SoulsCollected.text = "Souls Collected: " + GameManager.Instance.SoulsCollected;
        SoulsCorrupted.text = "Souls Corrupted: " + GameManager.Instance.SoulsCorrupted;
    }
}
