using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int initialSpawnCount = 6;
    public string killCondition = "J";
    private string[] enemyNames = new string[] {"John", "Paul", "Kris", "Josh","Ashley","Sean", "Petr"};
    public Transform[] spawnPoints;
    public GameObject[] enemyTypes;
    public List<GameObject> enemies;

    public int EnemyCount => enemies.Count;
    public bool NoEnemies => enemies.Count == 0;

    void Start()
    {
        SpawnEnemies();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            KillRandomEnemy();
        if (Input.GetKeyDown(KeyCode.J))
            KillSpecificEnemy(killCondition);
        if (Input.GetKeyDown(KeyCode.H))
            KillAllEnemies();
    }

    /// <summary>
    /// Spawns enemies up until the inital spawn count is met
    /// </summary>
    private void SpawnEnemies()
    {
        for(int i=0; i < initialSpawnCount; i++)
        {
            SpawnEnemy();
        }
    }

    /// <summary>
    /// Spawns a single enemy into our scene at random
    /// </summary>
    private void SpawnEnemy()
    {
        int rndEnemy = Random.Range(0, enemyTypes.Length);
        int rndSpawn = Random.Range(0, spawnPoints.Length);
        int rndName = Random.Range(0, enemyNames.Length);
        GameObject enemy = Instantiate(enemyTypes[rndEnemy], spawnPoints[rndSpawn].transform.position, spawnPoints[rndSpawn].transform.rotation);
        enemy.name = enemyNames[rndName];
        enemies.Add(enemy);
    }

    /// <summary>
    /// Kills a enemy
    /// </summary>
    /// <param name="_enemy">The enemy we want to kill</param>
    private void KillEnemy(GameObject _enemy)
    {
        if (NoEnemies)
            return;

        Destroy(_enemy);
        enemies.Remove(_enemy);
    }

    /// <summary>
    /// Kills a random enemy in our scene
    /// </summary>
    private void KillRandomEnemy()
    {
        if (NoEnemies)
            return;

        int rndEnemy = Random.Range(0, EnemyCount);
        KillEnemy(enemies[rndEnemy]); 
    }

    /// <summary>
    /// Kills a specific enemy in our scene that meets a specified condition
    /// </summary>
    /// <param name="_condition">The condition to check against</param>
    private void KillSpecificEnemy(string _condition)
    {
        if (NoEnemies)
            return;

        for(int i=0; i< EnemyCount; i++)
        {
            if (enemies[i].name.Contains(_condition))
            {
                KillEnemy(enemies[i]);
            }
        }
    }

    /// <summary>
    /// Kills all enemies in our scene
    /// </summary>
    private void KillAllEnemies()
    {
        if (NoEnemies)
            return;

        for(int i = EnemyCount - 1; i>= 0; i--)
            KillEnemy(enemies[i]);
    }
}
