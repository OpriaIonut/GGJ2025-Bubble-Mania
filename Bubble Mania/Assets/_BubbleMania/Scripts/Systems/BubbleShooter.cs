using UnityEngine;

namespace BubbleMania
{
    public class BubbleShooter : MonoBehaviour
    {
        [SerializeField] private GameObject bubblePrefab;
        [SerializeField] private Transform firePoint;

        [Header("Bullet Properties")]
        [SerializeField] private float shootCooldown = 1.0f;
        [SerializeField] private float projectileSpeed = 7.0f;
        [SerializeField] private float projectileDamage = 5.0f;
        [SerializeField] private BubbleType startBubbleType = BubbleType.Red;

        private BubbleType bubbleType;
        private float lastShootTime = 0.0f;

        private void Awake()
        {
            Locator.RegisterService(this);
        }

        private void Start()
        {
            bubbleType = startBubbleType;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                bubbleType = BubbleType.Red;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                bubbleType = BubbleType.Blue;
            if(Input.GetKeyDown(KeyCode.Alpha3))
                bubbleType = BubbleType.Green;

            if(Input.GetButton("Fire1") && Time.time - lastShootTime > shootCooldown)
            {
                ShootBubble();
                lastShootTime = Time.time;
            }
        }

        private void ShootBubble()
        {
            GameObject clone = Instantiate(bubblePrefab);
            clone.transform.position = firePoint.position;

            BubbleProjectile projectile = clone.GetComponent<BubbleProjectile>();
            projectile.Initialize(bubbleType, projectileSpeed, projectileDamage, firePoint.forward);
        }
    }
}