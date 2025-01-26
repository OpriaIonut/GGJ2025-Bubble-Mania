using UnityEngine;

namespace BubbleMania
{
    public enum WavePhase
    {
        Chill,
        Gradual
    }

    public class EnemyWaveManager : MonoBehaviour
    {
        [Header("WaveManagement")]
        [SerializeField] private float chillPhaseDuration = 60.0f;
        [SerializeField] private float gradualPhaseDuration = 150.0f;

        [SerializeField] private float enemyDamageIncreaseMulti = 1.5f;
        [SerializeField] private float enemyHpIncreaseMulti = 1.5f;
        [SerializeField] private float gradualEnemyIncreaseCooldown = 10;
        [SerializeField] private float gradualPhaseSpawnDelay = 2.0f;

        [Header("Burst")]
        [SerializeField] private Vector2 burstCooldownTimes = new Vector2(60.0f, 120.0f);
        [SerializeField] private float burstCountIncreaseCooldown = 240.0f;
        [SerializeField] private Vector2Int enemiesPerBurst = new Vector2Int(20, 40);
        [SerializeField] private float burstCenterDistFromPlayer = 50.0f;
        [SerializeField] private float burstEnemySpawnRadius = 15.0f;

        [SerializeField] private float endCycleEnemyDecrementMulti = 0.65f;

        [Header("StartState")]
        [SerializeField] private float startSpawnDelay = 3.0f;
        [SerializeField] private float startEnemyHealth = 5.0f;
        [SerializeField] private float startEnemyDamage = 5.0f;

        [SerializeField] private Vector2 spawnDistFromPlayer;
        [SerializeField] private GameObject enemyPrefab;

        private float lastSpawnTime = 0.0f;

        private WavePhase currentPhase = WavePhase.Chill;
        private float phaseStartTime = 0.0f;

        private float currentSpawnDelay;
        private int enemiesToSpawn = 2;
        private float enemySpeed = 3.5f;
        private float enemyHealth;
        private float enemyDamage;

        private int burstCount = 1;

        private bool firstAttributeIncrease = false;
        private bool isGamePaused = false;

        private float lastEnemyCountIncreaseTime = 0.0f;
        private float lastBurstCountIncreaseTime = 0.0f;
        private float lastBurstSpawnTime = 0.0f;
        private float burstSpawnCooldown = 0.0f;

        private Transform playerTransf;
        private TimerSystem timer;

        private void Start()
        {
            currentSpawnDelay = startSpawnDelay;
            enemyHealth = startEnemyHealth;
            enemyDamage = startEnemyDamage;

            burstSpawnCooldown = Random.Range(burstCooldownTimes.x, burstCooldownTimes.y);

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

            if(currentPhase == WavePhase.Chill)
            {
                if(timer.GameTime - phaseStartTime > chillPhaseDuration)
                {
                    //The first time around, don't increase enemy attributes
                    if (!firstAttributeIncrease)
                        IncreaseEnemyAttributes();
                    else
                        firstAttributeIncrease = true;

                    currentPhase = WavePhase.Gradual;
                    phaseStartTime = timer.GameTime;
                    currentSpawnDelay = gradualPhaseSpawnDelay;
                }
            }
            else
            {
                CalculateEnemyCountIncreaseFactors();
                if(timer.GameTime - phaseStartTime > gradualPhaseDuration)
                {
                    enemiesToSpawn = (int)Mathf.Floor(enemiesToSpawn * endCycleEnemyDecrementMulti);
                    currentPhase = WavePhase.Chill;
                    phaseStartTime = timer.GameTime;
                    currentSpawnDelay = startSpawnDelay;
                }
            }

            if(timer.GameTime - lastBurstSpawnTime > burstSpawnCooldown)
            {
                SpawnBurst();
                lastBurstSpawnTime = timer.GameTime;
                burstSpawnCooldown = Random.Range(burstCooldownTimes.x, burstCooldownTimes.y);
            }

            if(timer.GameTime - lastBurstCountIncreaseTime > burstCountIncreaseCooldown)
            {
                burstCount++;
                lastBurstCountIncreaseTime = timer.GameTime;
            }

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

                SpawnEnemyAtPosition(playerTransf.position + randPos + Vector3.up * 1.5f);
            }
        }

        private void SpawnBurst()
        {
            for(int index = 0; index < burstCount; ++index)
            {
                int spawnCount = Random.Range(enemiesPerBurst.x, enemiesPerBurst.y);

                float burstSpawnAngle = Random.Range(0.0f, 2.0f * Mathf.PI);
                Vector3 burstCenterPos = new Vector3(Mathf.Sin(burstSpawnAngle), 0.0f, Mathf.Cos(burstSpawnAngle));
                burstCenterPos = playerTransf.position + burstCenterPos * burstCenterDistFromPlayer;

                for(int index2 = 0; index2 < spawnCount; ++index2)
                {
                    float enemySpawnAngle = Random.Range(0.0f, 2.0f * Mathf.PI);
                    float enemyDistFromCenter = Random.Range(0.0f, burstEnemySpawnRadius);

                    Vector3 enemyPos = new Vector3(Mathf.Sin(enemySpawnAngle), 0.0f, Mathf.Cos(enemySpawnAngle));
                    enemyPos = burstCenterPos + enemyPos * enemyDistFromCenter;

                    SpawnEnemyAtPosition(enemyPos + Vector3.up * 1.5f);
                }
            }
        }

        private void SpawnEnemyAtPosition(Vector3 pos)
        {
            GameObject clone = Instantiate(enemyPrefab);
            clone.transform.position = pos;

            EnemyController enemyController = clone.GetComponent<EnemyController>();
            BubbleType type = (BubbleType)Random.Range(0, (int)BubbleType.Count);
            enemyController.InitializeEnemy(type, enemySpeed, enemyHealth, startEnemyDamage);
        }

        private void OnGamePaused(bool isPaused)
        {
            isGamePaused = isPaused;
        }

        private void IncreaseEnemyAttributes()
        {
            //enemyHealth *= enemyHpIncreaseMulti;
            //enemyDamage *= enemyDamage;
        }

        private void CalculateEnemyCountIncreaseFactors()
        {
            if(timer.GameTime - lastEnemyCountIncreaseTime > gradualEnemyIncreaseCooldown)
            {
                enemiesToSpawn++;
                lastEnemyCountIncreaseTime = timer.GameTime;
            }
        }
    }
}