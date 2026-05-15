using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class enemySpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject[] enemyPrefabs;
    public Transform player;

    [Header("Spawning Settings")]
    public int enemiesToSpawn = 100;
    public float spawnRadius = 20f;
    public float spawnInterval = 0.5f;

   
    public int enemiesPerLevel = 50;
   
    public float intervalMultiplierPerLevel = 0.75f;

    // List to keep track of all active enemies
    private List<enemy> activeEnemies = new List<enemy>();
    private float spawnTimer;
    private int   scaledEnemyCap;
    private float scaledInterval;

    private void Start()
    {
        // Build index 1 = level 1, 2 = level 2
        int level = Mathf.Max(0, SceneManager.GetActiveScene().buildIndex - 1);
        scaledEnemyCap = enemiesToSpawn + enemiesPerLevel * level;
        scaledInterval = spawnInterval * Mathf.Pow(intervalMultiplierPerLevel, level);
    }

    void Update()
    {
        // Clean up destroyed enemies so new ones can spawn to replace them
        activeEnemies.RemoveAll(e => e == null);

        float difficulty = GameTimer.Instance != null ? GameTimer.Instance.DifficultyMultiplier : 1f;
        float timedInterval = scaledInterval / difficulty;

        if (activeEnemies.Count < scaledEnemyCap)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= timedInterval)
            {
                SpawnEnemy();
                spawnTimer = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        // Centralized movement update for every enemy
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if (activeEnemies[i] != null)
            {
                activeEnemies[i].MoveTowardsPlayer();
            }
        }
    }

    void SpawnEnemy()
    {
        // Pick a random position in a circle around the spawner
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = new Vector3(randomCircle.x, -1f, randomCircle.y) + transform.position;

        // Pick a random enemy type
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Instantiate and Initialize
        GameObject newEnemyObj = Instantiate(prefab, spawnPos, Quaternion.identity);
        enemy newEnemy = newEnemyObj.GetComponent<enemy>();

        newEnemy.Initialize(player);
        activeEnemies.Add(newEnemy);
    }
}
