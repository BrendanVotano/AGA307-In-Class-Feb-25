using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EnemyType
{
    OneHanded, TwoHanded, Archer
}
public enum PatrolType { Linear, PingPong, Random}
public class EnemyManager : Singleton<EnemyManager>
{
    public int initialSpawnCount = 6;
    public float initialSpawnDelay = 1f;
    public string killCondition = "J";
    private string[] enemyNames = new string[] {"John", "Paul", "Kris", "Josh","Ashley","Sean", "Petr"};
    public Transform[] spawnPoints;
    public GameObject[] enemyTypes;
    public List<Enemy> enemies;

    public int EnemyCount => enemies.Count;
    public bool NoEnemies => enemies.Count == 0;

    public Transform GetRandomSpawnPoint => spawnPoints[Random.Range(0, spawnPoints.Length)];
    public Transform GetSpecificSpawnPoint(int _spawnPoint) => spawnPoints[_spawnPoint];

    void Start()
    {
        StartCoroutine(SpawnWithDelay(initialSpawnCount, initialSpawnDelay));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            KillRandomEnemy();
        if (Input.GetKeyDown(KeyCode.J))
            KillSpecificEnemy(killCondition);
        if (Input.GetKeyDown(KeyCode.H))
            KillAllEnemies();
        if (Input.GetKeyDown(KeyCode.C))
            KillClosestEnemy();
    }

    /// <summary>
    /// Spawns enemies with a delay until the initial spawn count is met
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnWithDelay(int _spawnCount, float _spawnDelay)
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            yield return new WaitForSeconds(_spawnDelay);
            if (_CurrentGameState == GameState.Playing)
                SpawnEnemy();
        }
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
        if(enemy.GetComponent<Enemy>() != null)
            enemy.GetComponent<Enemy>().Initialize(GetRandomSpawnPoint, enemy.name);
        enemies.Add(enemy.GetComponent<Enemy>());
        _UI.UpdateEnemyCount(EnemyCount);
    }

    /// <summary>
    /// Kills a enemy
    /// </summary>
    /// <param name="_enemy">The enemy we want to kill</param>
    private void KillEnemy(Enemy _enemy)
    {
        if (NoEnemies)
            return;

        GameManager.instance.AddScore(_enemy.MyScore);
        Destroy(_enemy.gameObject);
        enemies.Remove(_enemy);
        _UI.UpdateEnemyCount(EnemyCount);
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

    private void KillClosestEnemy()
    {
        Transform closest = GetClosestObject(_PLAYER.transform, enemies);
        print(closest.name);
        KillEnemy(closest.GetComponent<Enemy>());
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
