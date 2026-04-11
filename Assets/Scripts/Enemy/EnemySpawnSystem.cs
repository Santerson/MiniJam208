using NUnit.Framework.Internal;
using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    // Get Enemy Prefab
    [SerializeField] GameObject[] Enemys = new GameObject[4];
    Transform player; // to make sure its not near player
    float minRadius = 15f; // the min of spawning distance
    float maxRadius = 25f; // the maximum of spawning distance
    [SerializeField] float maxSpawnEnemiesPerWave = 50f;
    [SerializeField] float spawnedEnemiesInWave = 0f;
    float WaveTimer = 30f;
    float ResetWaveTimer;

    private void Awake()
    {
        ResetWaveTimer = WaveTimer;
        WaveTimer = 5f;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


    private void Update()
    {
        WaveTimer -= Time.deltaTime;
        if (WaveTimer <= 0)
        {
            SpawnEnemy();
            if (spawnedEnemiesInWave == maxSpawnEnemiesPerWave)
            {
                WaveTimer = ResetWaveTimer;
            }
        }
    }

    void SpawnEnemy()
    {

        // Pickinga Random Angle
        float angle = Random.Range(0, Mathf.PI * 2);

        // Pick a random distance in the inner and outer radius
        float radius = Random.Range(minRadius, maxRadius);

        // Contert polar coordinats(x, y)
        Vector2 spawnOffset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

        // Final position relative to player
        Vector2 spawnPosition = (Vector2)player.transform.position + spawnOffset;

        // Spawning Enemy
        GameObject Enemy = Instantiate(Enemys[Random.Range(0, Enemys.Length)], spawnPosition, Quaternion.identity);
        spawnedEnemiesInWave++;


        if (Random.Range(1, 100) <= 35)
        {
            Enemy.tag = "BiggerBadderEnemy";
        }
        else
        {
                Enemy.tag = "Enemy";
        }
    }
}
