using UnityEngine;

namespace BubbleMania
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform playerTransf;
        [SerializeField] private float cameraSpeed = 1.0f;

        private Vector3 offset;

        private void Start()
        {
            offset = transform.position - playerTransf.position;
        }

        private void Update()
        {
            Vector3 targetPos = playerTransf.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, cameraSpeed * Time.deltaTime);
        }
    }
}