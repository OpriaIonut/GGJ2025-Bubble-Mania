using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        [Header("UI")]
        [SerializeField] private List<Image> bubbleControlImages;

        private bool passThroughEnemies = true;
        private BubbleType bubbleType;
        private float lastShootTime = 0.0f;

        private void Awake()
        {
            Locator.RegisterService(this);
        }

        private void Start()
        {
            bubbleType = startBubbleType;
            UpdateBubbleSelectionUI();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                bubbleType = BubbleType.Red;
                UpdateBubbleSelectionUI();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                bubbleType = BubbleType.Blue;
                UpdateBubbleSelectionUI();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                bubbleType = BubbleType.Green;
                UpdateBubbleSelectionUI();
            }

            if (Input.GetKeyDown(KeyCode.N))
                passThroughEnemies = !passThroughEnemies;

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
            projectile.Initialize(bubbleType, projectileSpeed, projectileDamage, firePoint.forward, passThroughEnemies);
        }

        private void UpdateBubbleSelectionUI()
        {
            for(int index = 0; index < bubbleControlImages.Count; ++index)
            {
                bubbleControlImages[index].enabled = true;
            }

            int currentBubble = (int)bubbleType;
            bubbleControlImages[currentBubble].enabled = false;
        }
    }
}