using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BubbleMania
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 1.0f;
        [SerializeField] private bool globalMovement = true;
        [SerializeField] private float maxHealth = 100.0f;
        [SerializeField] private Animator anim;

        [Header("UI")]
        [SerializeField] private RectTransform healthbar;
        [SerializeField] private GameObject gameOverPanel;

        private bool isGamePaused = false;
        private float currentHealth;

        private void Awake()
        {
            Locator.RegisterService(this);
        }

        private void Start()
        {
            currentHealth = maxHealth;

            PauseSystem pauseSystem = Locator.GetService<PauseSystem>();
            pauseSystem.AddListener_OnGamePaused(OnGamePaused);
        }

        private void Update()
        {
            if (isGamePaused)
                return;

            if(Input.GetKeyDown(KeyCode.M))
            {
                globalMovement = !globalMovement;
            }

            RotateByMouse();
            Move();
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            healthbar.localScale = new Vector3(Mathf.Clamp01(currentHealth / maxHealth), 1.0f, 1.0f);

            if (currentHealth <= 0.0f)
                Die();
        }

        private void RotateByMouse()
        {
            float lookAtX = Input.mousePosition.x - Screen.width / 2.0f;
            float lookAtZ = Input.mousePosition.y - Screen.height / 2.0f;
            transform.LookAt(transform.position + new Vector3(lookAtX, 0.0f, lookAtZ));
        }

        private void Move()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if(horizontal != 0.0f || vertical != 0.0f)
            {
                anim.SetBool("Running", true);
                if (globalMovement)
                {
                    transform.position += new Vector3(horizontal, 0.0f, vertical).normalized * movementSpeed * Time.deltaTime;
                }
                else
                {
                    transform.position += (transform.forward * vertical + transform.right * horizontal).normalized * movementSpeed * Time.deltaTime;
                }
            }
            else
            {
                anim.SetBool("Running", false);
            }
        }

        private void Die()
        {
            gameOverPanel.SetActive(true);

            PauseSystem pauseSystem = Locator.GetService<PauseSystem>();
            pauseSystem.ToggleGamePause(false);

            StartCoroutine(ReloadScene());
        }

        private IEnumerator ReloadScene()
        {
            yield return new WaitForSeconds(5.0f);
            SceneManager.LoadScene("GameMap");
        }

        private void OnGamePaused(bool isPaused)
        {
            isGamePaused = isPaused;
        }
    }
}
