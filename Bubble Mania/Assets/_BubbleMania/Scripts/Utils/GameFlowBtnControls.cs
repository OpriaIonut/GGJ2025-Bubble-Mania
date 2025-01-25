using UnityEngine;
using UnityEngine.SceneManagement;

namespace BubbleMania
{
    public class GameFlowBtnControls : MonoBehaviour
    {
        public void _StartGame()
        {
            SceneManager.LoadScene("GameMap");
        }

        public void _TitleMenu()
        {
            SceneManager.LoadScene("TitleMenu");
        }

        public void _QuitGame()
        {
            Application.Quit();
        }
    }
}