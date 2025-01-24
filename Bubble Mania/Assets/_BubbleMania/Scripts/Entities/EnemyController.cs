using UnityEngine;

namespace BubbleMania
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 5.0f;
        [SerializeField] private BubbleType enemyType;
        [SerializeField] private MeshRenderer enemyGFX;

        private Transform player;

        private void Start()
        {
            player = Locator.GetService<PlayerController>().transform;
        }

        private void Update()
        {
            Move();
        }

        public void InitializeEnemy(BubbleType type, float speed)
        {
            enemyType = type;
            movementSpeed = speed;

            EnemyFactory enemyFactory = Locator.GetService<EnemyFactory>();
            enemyGFX.material.color = enemyFactory.GetColorByType(enemyType);
        }

        private void Move()
        {
            Vector3 moveDir = (player.position - transform.position).normalized;
            transform.LookAt(transform.position + moveDir);
            transform.position += moveDir * movementSpeed * Time.deltaTime;
        }
    }
}