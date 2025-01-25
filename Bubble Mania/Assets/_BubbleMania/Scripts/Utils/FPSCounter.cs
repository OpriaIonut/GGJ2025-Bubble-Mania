using TMPro;
using UnityEngine;

namespace _BubbleMania.Scripts.Utils {
    public class FPSCounter : MonoBehaviour {
        [SerializeField]
        private TextMeshProUGUI fpsText; // Assign this in the Inspector

        private float deltaTime = 0.0f;

        void Update() {
            // Calculate the frame time
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

            // Update the FPS text periodically
            if (fpsText != null) {
                float fps = 1.0f / deltaTime;
                fpsText.text = $"{fps:0.} FPS";
            }
        }
    }
}