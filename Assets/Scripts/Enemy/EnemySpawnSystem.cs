using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    // Get Enemy Prefab
    [SerializeField] private GameObject EnemyPrefab;

    // Spawn time of Enemy
    float SpawnTime = 10f;
    private void Update()
    {
        SpawnTime -= Time.deltaTime;

        // Spawn Enemies when SpawnTime is less than 0
        if(SpawnTime < 0)
        {
            SpawnTime = 10;
            Instantiate(EnemyPrefab);
        }
    }
}
