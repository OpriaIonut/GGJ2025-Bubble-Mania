using UnityEngine;
using Random = System.Random;

namespace _BubbleMania.Scripts.Utils {
    public class DistributeMaterials : MonoBehaviour {
        [SerializeField]
        private Material defaultMat;

        [SerializeField]
        private Mesh defaultMesh;

        [SerializeField]
        private Material highlightMat;

        [SerializeField]
        private Mesh highlightMesh;

        [SerializeField]
        [Range(0, 100)]
        private float highlightPercent;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [ContextMenu("Distribute materials")]
        public void Distribute() {
            Random random = new Random();
            var meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers) {
                var useHighlight = random.Next(100) < highlightPercent;
                meshRenderer.material = useHighlight ? highlightMat : defaultMat;
                meshRenderer.GetComponent<MeshFilter>().mesh = useHighlight ? highlightMesh : defaultMesh;
                if (useHighlight) {
                    meshRenderer.transform.parent.localRotation = Quaternion.Euler(0, ((int)random.Next(0, 6)) * 60, 0);
                }
            }
        }
    }
}