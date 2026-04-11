using NUnit.Framework.Internal;
using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    // Get Enemy Prefab
    [SerializeField] GameObject[] Enemys = new GameObject[4];
    Transform player; // to make sure its not near player
    float minRadius = 15f; // the min of spawning distance
    float maxRadius = 25f; // the maximum of spawning distance

    [Tooltip ("FIX THESE FUCKING NAMES (BISHEP)")]
    [Header("Wave/Spawning")]
    [SerializeField] float StartSpawnedEnemies = 8f;
    [SerializeField] float spawnedStartEnemies = 0f;
    [SerializeField] float maxSpawnEnemiesPerWave = 15f;
    [SerializeField] float spawnedEnemiesInWave = 0f;
    float IntervalBetweenWaves = 60f;
    float WaveTimer = 5;
    float ResetWaveTimer;
    float ResetIntervalBetweenWaves;
    float RegualSpawnTime = 2f;
    float ResetSpawnTime;
    [SerializeField] float RegualMaxSpawn = 50f;
    public float RegularEnemiesSpawn = 0;
    bool spawnedStartEnemiesBool = false;

    private void Awake()
    {
        ResetWaveTimer = WaveTimer;
        ResetIntervalBetweenWaves = IntervalBetweenWaves;
        ResetSpawnTime = RegualSpawnTime;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


    private void Update()
    {
        if (spawnedStartEnemiesBool == false)
        {
            SpawnEnemy();
        }
        else if (IntervalBetweenWaves <= 0f) // for after the Inbetween waves
        {
            WaveTimer -= Time.deltaTime; // Wave Countdown
            if (WaveTimer <= 0f)
            {
                SpawnEnemy();
            }
        }
        else
        {
            if (RegualSpawnTime <= 0 && RegularEnemiesSpawn != RegualMaxSpawn) // Regular Version
            {
                SpawnEnemy();
                RegularEnemiesSpawn++;
                RegualSpawnTime = ResetSpawnTime;
                IntervalBetweenWaves -= Time.deltaTime;
            }
            RegualSpawnTime -= Time.deltaTime;
        }
        if (WaveTimer <= 0 && spawnedEnemiesInWave == maxSpawnEnemiesPerWave) // to fix the WaveTimer after its done
        {
            WaveTimer = ResetWaveTimer;
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

        if (spawnedStartEnemies < StartSpawnedEnemies) // for the starting spawned enemies
        {
            spawnedStartEnemies++;
        }
        if (spawnedStartEnemies >= StartSpawnedEnemies) // to do Waves spawning
        {
            spawnedStartEnemiesBool = true;
            if (IntervalBetweenWaves == 0)
            {
                if (spawnedEnemiesInWave > maxSpawnEnemiesPerWave)
                {
                    spawnedEnemiesInWave++;
                }
                if (spawnedEnemiesInWave == maxSpawnEnemiesPerWave)
                {
                    spawnedEnemiesInWave = 0;
                    IntervalBetweenWaves = ResetIntervalBetweenWaves;
                }
            }
        }


        if (Random.Range(1, 100) <= 35) // so the can spawn enemies
        {
            Enemy.tag = "BiggerBadderEnemy";
        }
        else
        {
            Enemy.tag = "Enemy";
        }
    }
}
