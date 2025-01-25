using UnityEngine;

namespace BubbleMania
{
    public class BubbleProjectile : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRend;

        private bool isInitialized = false;
        private float speed;
        private float damage;
        private BubbleType type;
        private Vector3 moveDir;

        public void Initialize(BubbleType _type, float _speed, float _damage, Vector3 _moveDir)
        {
            isInitialized = true;
            type = _type;
            speed = _speed;
            damage = _damage;
            moveDir = _moveDir;

            EnemyFactory enemyFactory = Locator.GetService<EnemyFactory>();
            meshRend.material.color = enemyFactory.GetColorByType(type);

            Destroy(gameObject, 10.0f);
        }

        private void Update()
        {
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
                if(controller.EnemyType == type)
                {
                    controller.TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
        }
    }
}