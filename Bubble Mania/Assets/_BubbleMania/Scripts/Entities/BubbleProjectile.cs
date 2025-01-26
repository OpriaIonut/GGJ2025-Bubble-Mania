using UnityEngine;

namespace BubbleMania
{
    public class BubbleProjectile : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRend;
        [SerializeField] private GameObject hitVfx;

        private bool isInitialized = false;
        private float speed;
        private float damage;
        private bool passThroughEnemies;
        private BubbleType type;
        private Vector3 moveDir;

        private bool isGamePaused = false;

        private void Start()
        {
            PauseSystem pauseSystem = Locator.GetService<PauseSystem>();
            pauseSystem.AddListener_OnGamePaused(OnGamePaused);
        }

        public void Initialize(BubbleType _type, float _speed, float _damage, Vector3 _moveDir, bool _passThroughEnemies)
        {
            isInitialized = true;
            type = _type;
            speed = _speed;
            passThroughEnemies = _passThroughEnemies;
            damage = _damage;
            moveDir = _moveDir;

            EnemyFactory enemyFactory = Locator.GetService<EnemyFactory>();
            // pick the color but make alpha 10%
            var colorByType = enemyFactory.GetColorByType(type);
            colorByType.a = 0.4f;
            meshRend.material.color = colorByType;

            Destroy(gameObject, 10.0f);
        }

        private void Update()
        {
            if (isGamePaused)
                return;

            if(isInitialized)
            {
                transform.position += moveDir * speed * Time.deltaTime;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.root.tag == "Enemy")
            {
                EnemyController controller = other.transform.root.GetComponent<EnemyController>();
                if (controller.EnemyType == type)
                {
                    controller.TakeDamage(damage);
                    DestroyProjectile();
                }
                else if (!passThroughEnemies)
                    DestroyProjectile();
            }
            else if(other.transform.root.tag != "Player" && other.transform.tag != "IgnoreProjectileHit")
                DestroyProjectile();
        }

        private void OnGamePaused(bool isPaused)
        {
            isGamePaused = isPaused;
        }

        private void DestroyProjectile()
        {
            GameObject clone = Instantiate(hitVfx);
            clone.transform.position = transform.position;
            ParticleSystem particle = clone.GetComponent<ParticleSystem>();
            var module = particle.main;

            Color newCol = meshRend.material.color;
            newCol.a = 1.0f;
            module.startColor = new ParticleSystem.MinMaxGradient(newCol);
            module.startSpeed = 5.0f;
            Destroy(clone, 2.0f);

            PauseSystem pauseSystem = Locator.GetService<PauseSystem>();
            pauseSystem.RemoveListener_OnGamePaused(OnGamePaused);
            Destroy(gameObject);
        }
    }
}