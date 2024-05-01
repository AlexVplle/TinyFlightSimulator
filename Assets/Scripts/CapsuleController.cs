using Unity.Netcode;
using UnityEngine;

public class CapsuleController : NetworkBehaviour
{
    public float speed = 5f;           // Speed of the capsule's movement
    public float turnSpeed = 180f;     // Turning speed in degrees per second

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        // Get input from the keyboard
        float horizontal = Input.GetAxis("Horizontal");   // A/D or Left/Right Arrow keys
        float vertical = Input.GetAxis("Vertical");       // W/S or Up/Down Arrow keys

        // Move the capsule forward and backward
        transform.Translate(Vector3.forward * vertical * speed * Time.deltaTime);

        // Rotate the capsule based on horizontal input
        transform.Rotate(Vector3.up, horizontal * turnSpeed * Time.deltaTime);
    }
}
