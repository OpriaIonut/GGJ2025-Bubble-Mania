using UnityEngine;

namespace BubbleMania
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 1.0f;
        [SerializeField] private bool globalMovement = true;

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            float lookAtX = Input.mousePosition.x - Screen.width / 2.0f;
            float lookAtZ = Input.mousePosition.y - Screen.height / 2.0f;

            transform.LookAt(transform.position + new Vector3(lookAtX, 0.0f, lookAtZ));

            if(globalMovement)
            {
                transform.position += new Vector3(horizontal, 0.0f, vertical) * movementSpeed * Time.deltaTime;
            }
            else
            {
                transform.position += (transform.forward * vertical + transform.right * horizontal).normalized * movementSpeed * Time.deltaTime;
            }
        }
    }
}
