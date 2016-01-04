using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    SpawnManager manager;

    Wave[] waves;
    int type = 2; // 1 = walker, 2 = sprinter

    public Enemy spawn;
    public Transform[] spawners;

    Transform spawnPoint;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    bool RoundCheck() {
        if (manager.GetRoundComplete()) return true;
        else return false;
    }

    void Start() {

        manager = GetComponentInParent<SpawnManager>();
        manager.Start();

        for (int i = 0; i <= transform.childCount - 1; i++)
            spawners[i] = transform.GetChild(i);

        if (spawn.name == "Walker") { 
            type = 1;
            //manager.Start();
        }
        SetWaves();
        NextWave();
    }

    void Update() {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime && Time.time > currentWave.delay) {

            Debug.Log("WE IN! " + gameObject.name);

            FindSpawnPoint();
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            Enemy spawnedEnemy = Instantiate(spawn, spawnPoint.position, Quaternion.identity) as Enemy;
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
        else if (enemiesRemainingAlive == 0 && currentWaveNumber != waves.Length) {
            NextWave();
        }
        if (RoundCheck()) {
            Debug.Log(gameObject.name + " round complete");
            SetWaves();
            NextWave();
        }
    }

    void OnEnemyDeath() {
        enemiesRemainingAlive--;
        Debug.Log(gameObject.name + " was killed");
        if (enemiesRemainingAlive == 0 && currentWaveNumber >= waves.Length) {
            Debug.Log(gameObject.name + " round ended");
            manager.RoundEnd(type);
        }
        else if (enemiesRemainingAlive == 0) {
            NextWave();
        }
    }

    void NextWave() {
        if (currentWaveNumber < waves.Length) {
            currentWave = waves[currentWaveNumber];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
            currentWave.delay += Time.time;

            Debug.Log("Round: " + manager.GetRoundNum() + "Wave: " + (currentWaveNumber + 1) + ". Enemies: " + enemiesRemainingToSpawn);
        }
        currentWaveNumber++;
        
    }

    void FindSpawnPoint() {
        int point = Random.Range(0, spawners.Length);
        spawnPoint = spawners[point];
    }

    void SetWaves() {
        Debug.Log("Waves set");
        waves = manager.GetWaves(type);
        currentWaveNumber = 0;
    }

    //[System.Serializable]
    public class Wave
    {
        public int enemyCount;

        // time in seconds
        public float timeBetweenSpawns;
        public float delay;
    }
}
