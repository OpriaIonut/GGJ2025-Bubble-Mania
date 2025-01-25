using UnityEngine;
using UnityEngine.SceneManagement;

namespace BubbleMania
{
    public class GameFlowBtnControls : MonoBehaviour
    {
        public void _StartGame()
        {
            SceneManager.LoadScene("NewMap");
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