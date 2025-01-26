using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _BubbleMania.Scripts.Utils {
    public class DistributePrefabs : MonoBehaviour {
        [SerializeField]
        private List<GameObject> prefabModels;

        [SerializeField]
        private Vector4 range;

        [SerializeField]
        private float count;

        [ContextMenu("Place items")]
        public void Place() {
            for (int i = 0; i < count; i++) {
                Vector3 position = new Vector3(Random.Range(range.x, range.y), 0, Random.Range(range.z, range.w));
                var rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                Instantiate(prefabModels[Random.Range(0, prefabModels.Count)], position, rotation,
                    transform);
            }
        }

        [ContextMenu("Clear items")]
        public void Clear() {
            List<GameObject> children = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++) {
                children.Add(transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < children.Count; i++) {
                DestroyImmediate(children[i]);
            }
        }
    }
}