using NUnit.Framework.Internal;
using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    // Get Enemy Prefab
    [SerializeField] GameObject[] Enemys = new GameObject[4];
    Transform player; // to make sure its not near player
    
    [Header("Spawning Distances")]
    [SerializeField] float minRadius = 15f; // the min of spawning distance
    [SerializeField] float maxRadius = 25f; // the maximum of spawning distance

    [Header("General Spawn Settings")]
    [Tooltip("Chance for the enemy to be a soul enemy (0-100)")]
    [SerializeField] uint SoulEnemyChance = 35;
    [Tooltip("The maximum amount of enemies that can be on the map at once")]
    [SerializeField] int MaxEnemies = 50;

    [Header("Initial Spawns")]
    [Tooltip("Time until the initial enemies spawn in seconds")]
    [SerializeField] float TimeToInitialEnemySpawns = 10f;
    [Tooltip("The amount of enemies to spawn at the start of the game")]
    [SerializeField] int InitialEnemySpawns = 5;

    [Header("Regular Spawns")]
    [Tooltip("Time between regular enemy spawns in seconds (min and max)")]
    [SerializeField] Vector2 TimeInbetweenSpawns = new Vector2(2f, 4f);

    [Header("Waves")]
    [SerializeField] float IntervalBetweenWaves = 60f;
    [SerializeField] float WaveDuration = 5;
    [SerializeField] float SpawnRateInWave = 0.5f;
    [SerializeField] float TimeAfterWaveBeforeNextSpawn = 10f;

    [Header("Scaling BISHEP >:(")]


    float timeToNextEnemySpawn = 5f;
    float timeToNextWave = 60f;
    float timeLeftOfWave = 0f;

    // The amount of enemies currently on the field, used to make sure we don't spawn more than the max amount of enemies
    public int TotalEnemiesCurrentlySpawned = 0;
    // Whether or not the initial enemies have spawned
    bool spawnedStartEnemiesBool = false;
    // Whether or not we are currently spawning a wave
    bool spawningWave = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Start()
    {
        timeToNextEnemySpawn = Random.Range(TimeInbetweenSpawns.x, TimeInbetweenSpawns.y);
        timeToNextWave = IntervalBetweenWaves;
    }


    private void Update()
    {
        // Handle first enemy spawns
        if (!spawnedStartEnemiesBool)
        {
            // Decrement the spawn timer
            TimeToInitialEnemySpawns -= Time.deltaTime;
            // Spawn the initial enemies when the timer runs out
            if (TimeToInitialEnemySpawns <= 0)
            {
                // Spawn the initial enemies
                for (int i = 0; i < InitialEnemySpawns; i++)
                {
                    SpawnEnemy();
                }
                spawnedStartEnemiesBool = true;
            }
            // Do nothing else
            return;
        }

        // Decrement timers to the next enemy spawns and wave
        timeToNextEnemySpawn -= Time.deltaTime;
        timeToNextWave -= Time.deltaTime;

        // Start a wave if it's time to start one
        if (timeToNextWave <= 0 && !spawningWave)
        {
            // start the wave
            spawningWave = true;
            timeLeftOfWave = WaveDuration;
            // Change the enemy spawn timer to be faster during the wave
            timeToNextEnemySpawn = SpawnRateInWave;
        }

        // Handle wave spawining logic
        if (spawningWave)
        {
            // Deduct the time left in the wave
            timeLeftOfWave -= Time.deltaTime;

            // Spawn enemies at the wave spawn rate
            if (timeToNextEnemySpawn <= 0 && TotalEnemiesCurrentlySpawned < MaxEnemies)
            {
                SpawnEnemy();
                timeToNextEnemySpawn = SpawnRateInWave;
            }

            // End the wave if the wave duration is up
            if (timeLeftOfWave <= 0)
            {
                spawningWave = false;
                timeToNextWave = IntervalBetweenWaves;
                timeToNextEnemySpawn = TimeAfterWaveBeforeNextSpawn;
            }
        }
        else
        {
            // Otherwise, handle regular enemy spawns
            HandleGeneralEnemySpawns();
        }




        // ????????????????????????????????????????????????????`?????????????????????????????
        // COMMENT YOUR DAMN CODE
        // (It doens't even work)
        /*
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
                IntervalBetweenWaves -= Time.deltaTime; // THIS DOESN'T WORK
            }
            RegualSpawnTime -= Time.deltaTime;
        }
        if (WaveTimer <= 0 && spawnedEnemiesInWave == maxSpawnEnemiesPerWave) // to fix the WaveTimer after its done
        {
            WaveTimer = ResetWaveTimer;
        }
        */
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

        // Increment the total amount of enemies on the map
        TotalEnemiesCurrentlySpawned++;

        // Decide whether or not the enemy should drop a soul
        if (Random.Range(1, 100) <= SoulEnemyChance)
        {
            Enemy.tag = "BiggerBadderEnemy";
        }
        else
        {
            Enemy.tag = "Enemy";
        }


        // Wtf is this? COMMENT YOUR CODE DUDE
        //if (spawnedStartEnemies < StartSpawnedEnemies) // for the starting spawned enemies
        //{
        //    spawnedStartEnemies++;
        //}
        //if (spawnedStartEnemies >= StartSpawnedEnemies) // to do Waves spawning
        //{
        //    spawnedStartEnemiesBool = true;
        //    if (IntervalBetweenWaves == 0)
        //    {
        //        if (spawnedEnemiesInWave > maxSpawnEnemiesPerWave)
        //        {
        //            spawnedEnemiesInWave++;
        //        }
        //        if (spawnedEnemiesInWave == maxSpawnEnemiesPerWave)
        //        {
        //            spawnedEnemiesInWave = 0;
        //            IntervalBetweenWaves = ResetIntervalBetweenWaves;
        //        }
        //    }
        //}
    }

    void HandleGeneralEnemySpawns()
    {
        // Spawn an enemy if it's time to spawn one and we haven't reached the max amount of enemies on the map
        if (timeToNextEnemySpawn <= 0 && TotalEnemiesCurrentlySpawned < MaxEnemies)
        {
            SpawnEnemy();
            timeToNextEnemySpawn = Random.Range(TimeInbetweenSpawns.x, TimeInbetweenSpawns.y);
        }
    }
}
