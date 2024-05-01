using Unity.Netcode;
using UnityEngine;

public class FollowCamera : NetworkBehaviour
{
    public Transform target; // The target object to follow (your capsule)
    public Vector3 offset = new Vector3(0, 2, -5); // Offset from the target object, adjust as needed
    public float followSpeed = 10f; // Speed at which the camera follows the target

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            GetComponent<Camera>().enabled = false;
            return;
        }
    }

    void Start()
    {
        // Set the initial position of the camera
        if (target != null)
        {
            transform.position = target.position + target.TransformDirection(offset);
            transform.LookAt(target);
        }
    }

    void LateUpdate()
    {
        // Check if the target is not null
        if (target != null)
        {
            // Calculate the desired position of the camera
            Vector3 desiredPosition = target.position + target.TransformDirection(offset);
            // Smoothly move the camera to the desired position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
            // Make the camera look at the target
            transform.LookAt(target);
        }
        else
        {
            Debug.LogWarning("FollowCamera: No target specified!");
        }
    }
}
