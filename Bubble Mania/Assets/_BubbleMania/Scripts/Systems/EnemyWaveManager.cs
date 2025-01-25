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

        private bool isGamePaused = false;

        private Transform playerTransf;
        private TimerSystem timer;

        private void Start()
        {
            currentSpawnDelay = startSpawnDelay;
            enemiesToSpawn = startEnemiesToSpawn;
            enemySpeed = startEnemySpeed;

            timer = Locator.GetService<TimerSystem>();

            PauseSystem pauseSystem = Locator.GetService<PauseSystem>();
            pauseSystem.AddListener_OnGamePaused(OnGamePaused);

            PlayerController player = Locator.GetService<PlayerController>();
            playerTransf = player.transform;
        }

        private void Update()
        {
            if (isGamePaused)
                return;

            if(timer.GameTime - lastSpawnTime > currentSpawnDelay)
            {
                SpawnEnemies();
                lastSpawnTime = timer.GameTime;
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
                clone.transform.position = playerTransf.position + randPos + Vector3.up * 1.0f;

                EnemyController enemyController = clone.GetComponent<EnemyController>();
                BubbleType type = (BubbleType)Random.Range(0, (int)BubbleType.Count);
                enemyController.InitializeEnemy(type, enemySpeed, enemyHealth, startEnemyDamage);
            }
        }

        private void OnGamePaused(bool isPaused)
        {
            isGamePaused = isPaused;
        }
    }
}