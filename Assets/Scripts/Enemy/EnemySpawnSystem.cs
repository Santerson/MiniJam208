using NUnit.Framework.Internal;
using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    // Get Enemy Prefab
    [SerializeField] GameObject[] Enemys = new GameObject[4];
    Transform player; // to make sure its not near player
    float minRadius = 15f; // the min of spawning distance
    float maxRadius = 25f; // the maximum of spawning distance
    float TestSpawnInterval= 2f;
    public float TestTimer = 0;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


    private void Update()
    {
        TestTimer += Time.deltaTime;
        if (TestTimer >=  TestSpawnInterval)
        {
            SpawnEnemy();
            TestTimer = 0;
        }
    }

    void SpawnEnemy()
    {
        // Chance to change the to make the enemy change tag
        int chance = Random.Range(1, 5);

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


        if (chance <= 2)
        {
            Enemy.tag = "BiggerBadderEnemy";
        }
    }
}
