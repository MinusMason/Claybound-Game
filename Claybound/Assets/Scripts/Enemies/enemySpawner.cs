using UnityEngine;
using System.Collections.Generic;

public class enemySpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject enemyPrefab;
    public Transform player;

    [Header("Spawning Settings")]
    public int enemiesToSpawn = 100;
    public float spawnRadius = 20f;
    public float spawnInterval = 0.5f;

    // List to keep track of all active enemies
    private List<enemy> activeEnemies = new List<enemy>();
    private float spawnTimer;

    void Update()
    {
        float difficulty = GameTimer.Instance != null ? GameTimer.Instance.DifficultyMultiplier : 1f;
        float scaledInterval = spawnInterval / difficulty;

        if (activeEnemies.Count < enemiesToSpawn)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= scaledInterval)
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
        Vector3 spawnPos = new Vector3(randomCircle.x, 1f, randomCircle.y) + transform.position;

        // Instantiate and Initialize
        GameObject newEnemyObj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemy newEnemy = newEnemyObj.GetComponent<enemy>();

        newEnemy.Initialize(player);
        activeEnemies.Add(newEnemy);
    }
}
