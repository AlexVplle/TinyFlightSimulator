using System.Globalization;
using Unity.Netcode;
using UnityEngine;

namespace MFlight
{
    public class MouseFlightController : NetworkBehaviour
    {
        [Header("Components")]
        [SerializeField, Tooltip("Transform of the camera itself")]
        private Camera cam = null;

        [SerializeField, Tooltip("Transform of the plane")]
        private Transform planeTransform = null;

        [SerializeField, Tooltip("How quickly the camera tracks the mouse aim point.")]
        private float camSmoothSpeed = 5f;

        [SerializeField, Tooltip("Mouse sensitivity for the mouse flight target")]
        private float mouseSensitivity = 20f;

        [SerializeField, Tooltip("Distance from the plane")]
        private float distanceFromPlane = 10f;

        private float yaw = 0f;
        private float pitch = 0f;

        private void Awake()
        {
            // Validate that all required components are assigned
            ValidateComponents();
        }

        public override void OnNetworkSpawn()
        {
            // Disable the controller if it's not owned by the local player
            if (!IsOwner)
            {
                DisableController();
            }
        }

        private void Update()
        {
            // Process input and update flight controls only if this is the local player's controller
            HandleInput();
            UpdateCameraPosition();
        }

        private void ValidateComponents()
        {
            // Check if the camera component is assigned
            if (cam == null) Debug.LogError($"{name} MouseFlightController - No camera transform assigned!");
            // Check if the plane transform is assigned
            if (planeTransform == null) Debug.LogError($"{name} MouseFlightController - No plane transform assigned!");
        }

        private void DisableController()
        {
            // Disable the controller and camera if this is not the local player
            enabled = false;
            cam.enabled = false;
        }

        private void HandleInput()
        {
            // Handle mouse input for rotating the camera
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -45f, 45f); // Clamp the pitch to avoid excessive vertical rotation
        }

        private void UpdateCameraPosition()
        {
            // Calculate the camera's position relative to the plane
            Vector3 offset = new Vector3(0, 0, -distanceFromPlane);
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 targetPosition = planeTransform.position + rotation * offset;

            // Smoothly update the camera's position and rotation
            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, camSmoothSpeed * Time.deltaTime);
            cam.transform.LookAt(planeTransform.position);
        }
    }
}
