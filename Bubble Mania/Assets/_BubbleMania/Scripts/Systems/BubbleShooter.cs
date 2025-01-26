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

        private bool isGamePaused = false;
        private TimerSystem timer;

        private void Awake()
        {
            Locator.RegisterService(this);
        }

        private void Start()
        {
            bubbleType = startBubbleType;
            UpdateBubbleSelectionUI();

            timer = Locator.GetService<TimerSystem>();

            PauseSystem pauseSystem = Locator.GetService<PauseSystem>();
            pauseSystem.AddListener_OnGamePaused(OnGamePaused);
        }

        private void Update()
        {
            if (isGamePaused)
                return;

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

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0.0f)
            {
                int newIndex = (int)bubbleType;
                if(scroll < 0.0f)
                {
                    newIndex--;
                    if (newIndex < 0)
                        newIndex = (int)BubbleType.Count - 1;
                }
                else
                {
                    newIndex++;
                    if (newIndex >= (int)BubbleType.Count)
                        newIndex = 0;
                }
                bubbleType = (BubbleType)newIndex;
                UpdateBubbleSelectionUI();
            }

            if (Input.GetKeyDown(KeyCode.N))
                passThroughEnemies = !passThroughEnemies;

            if(Input.GetButton("Fire1") && timer.GameTime - lastShootTime > shootCooldown)
            {
                ShootBubble();
                lastShootTime = timer.GameTime;
            }
        }

        private void ShootBubble()
        {
            GameObject clone = Instantiate(bubblePrefab);
            clone.transform.position = firePoint.position;

            BubbleProjectile projectile = clone.GetComponent<BubbleProjectile>();
            projectile.Initialize(bubbleType, projectileSpeed, projectileDamage, transform.forward, passThroughEnemies);
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

        private void OnGamePaused(bool isPaused)
        {
            isGamePaused = isPaused;
        }
    }
}