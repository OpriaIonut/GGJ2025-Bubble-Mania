using UnityEngine;

namespace BubbleMania
{
    public class EnemyWaveManager : MonoBehaviour
    {
        [SerializeField] private float startSpawnDelay = 3.0f;
        [SerializeField] private int startEnemiesToSpawn = 5;
        [SerializeField] private float startEnemySpeed = 5.0f;
        [SerializeField] private float startEnemyHealth = 5.0f;
        [SerializeField] private float startEnemyDamage = 5.0f;

        [SerializeField] private Vector2 spawnDistFromPlayer;
        [SerializeField] private GameObject enemyPrefab;

        private float lastSpawnTime = 0.0f;
        private float currentSpawnDelay;
        private int enemiesToSpawn;
        private float enemySpeed;
        private float enemyHealth;

        private void Start()
        {
            currentSpawnDelay = startSpawnDelay;
            enemiesToSpawn = startEnemiesToSpawn;
            enemySpeed = startEnemySpeed;
        }

        private void Update()
        {
            if(Time.time - lastSpawnTime > currentSpawnDelay)
            {
                SpawnEnemies();
                lastSpawnTime = Time.time;
            }
        }

        private void SpawnEnemies()
        {
            for(int index = 0; index < enemiesToSpawn; ++index)
            {
                float angle = Random.Range(0.0f, 2.0f * Mathf.PI);
                float dist = Random.Range(spawnDistFromPlayer.x, spawnDistFromPlayer.y);
                Vector3 randPos = new Vector3(Mathf.Sin(angle), 0.0f, Mathf.Cos(angle)) * dist;

                GameObject clone = Instantiate(enemyPrefab);
                clone.transform.position = randPos;

                EnemyController enemyController = clone.GetComponent<EnemyController>();
                BubbleType type = (BubbleType)Random.Range(0, (int)BubbleType.Count);
                enemyController.InitializeEnemy(type, enemySpeed, enemyHealth, startEnemyDamage);
            }
        }
    }
}