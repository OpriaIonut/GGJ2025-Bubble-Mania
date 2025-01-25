using TMPro;
using UnityEngine;

namespace BubbleMania
{
    public class TimerSystem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;

        private float timeSinceGameStart = 0.0f;

        private bool isGamePaused = false;
        private int minutes = 0;
        private int seconds = 0;

        public float GameTime { get { return timeSinceGameStart; } }

        private void Awake()
        {
            Locator.RegisterService(this);
        }

        private void Start()
        {
            PauseSystem pauseSystem = Locator.GetService<PauseSystem>();
            pauseSystem.AddListener_OnGamePaused(OnGamePaused);
        }

        private void Update()
        {
            if (isGamePaused)
                return;

            timeSinceGameStart += Time.deltaTime;
            minutes = (int)Mathf.Floor(timeSinceGameStart / 60);
            seconds = (int)Mathf.Floor(timeSinceGameStart - minutes * 60);

            string time = minutes < 10 ? "0" : "";
            time += minutes + ":";
            time += seconds < 10 ? "0" : "";
            time += seconds;
            timerText.text = time;
        }

        private void OnGamePaused(bool isPaused)
        {
            isGamePaused = isPaused;
        }
    }
}