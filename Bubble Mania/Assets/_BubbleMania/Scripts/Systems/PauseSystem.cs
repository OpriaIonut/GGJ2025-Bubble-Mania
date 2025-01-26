using UnityEngine;
using UnityEngine.Events;

namespace BubbleMania
{
    public class PauseSystem : MonoBehaviour
    {
        [SerializeField] private GameObject gamePauseUI;
        [SerializeField] private GameObject pauseImg;
        [SerializeField] private GameObject playImg;

        private bool isGamePaused = false;
        private UnityAction<bool> onGamePaused;

        public bool IsGamePaused { get { return isGamePaused; } }

        private void Awake()
        {
            Locator.RegisterService(this);
        }

        public void ToggleGamePause(bool displayUI)
        {
            isGamePaused = !isGamePaused;
            onGamePaused?.Invoke(isGamePaused);

            pauseImg.SetActive(!isGamePaused);
            playImg.SetActive(isGamePaused);

            gamePauseUI.SetActive(false);
            if(displayUI && isGamePaused)
                gamePauseUI.SetActive(true);
        }

        public void HidePauseBtn()
        {
            pauseImg.transform.parent.gameObject.SetActive(false);
        }

        public void AddListener_OnGamePaused(UnityAction<bool> callback)
        {
            onGamePaused += callback;
        }
        public void RemoveListener_OnGamePaused(UnityAction<bool> callback)
        {
            onGamePaused -= callback;
        }
    }
}