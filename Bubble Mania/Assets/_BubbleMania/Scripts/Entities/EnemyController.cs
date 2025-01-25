using UnityEngine;

namespace BubbleMania
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 5.0f;
        [SerializeField] private BubbleType enemyType;
        [SerializeField] private MeshRenderer enemyGFX;

        [Header("UI")]
        [SerializeField] private GameObject healthbarParent;
        [SerializeField] private RectTransform healthbar;

        private Transform player;
        private float maxHealth;
        private float currentHealth;

        public BubbleType EnemyType { get { return enemyType; } }

        private void Start()
        {
            player = Locator.GetService<PlayerController>().transform;
        }

        private void Update()
        {
            Move();
        }

        public void InitializeEnemy(BubbleType type, float speed, float hp)
        {
            enemyType = type;
            movementSpeed = speed;

            maxHealth = hp;
            currentHealth = maxHealth;

            EnemyFactory enemyFactory = Locator.GetService<EnemyFactory>();
            enemyGFX.material.color = enemyFactory.GetColorByType(enemyType);
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0.0f)
                Die();
            else
                healthbarParent.transform.localScale = new Vector3(currentHealth / maxHealth, 1.0f, 1.0f);
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        private void Move()
        {
            Vector3 moveDir = (player.position - transform.position).normalized;
            transform.LookAt(transform.position + moveDir);
            transform.position += moveDir * movementSpeed * Time.deltaTime;
        }
    }
}