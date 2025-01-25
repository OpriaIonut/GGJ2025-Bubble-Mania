using UnityEngine;
using Random = System.Random;

namespace _BubbleMania.Scripts.Utils {
    public class DistributeMaterials : MonoBehaviour {
        [SerializeField]
        private Material defaultMat;

        [SerializeField]
        private Material highlightMat;

        [SerializeField]
        [Range(0, 100)]
        private float highlightPercent;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [ContextMenu("Distribute materials")]
        public void Distribute() {
            Random random = new Random();
            var meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers) {
                meshRenderer.material = random.Next(100) < highlightPercent ? highlightMat : defaultMat;
            }
        }
    }
}