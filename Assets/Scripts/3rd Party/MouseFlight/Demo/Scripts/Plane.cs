using System.ComponentModel.Design;
using Unity.Netcode;
using UnityEngine;

namespace MFlight.Demo
{
    [RequireComponent(typeof(Rigidbody))]
    public class Plane : NetworkBehaviour
    {
        [Header("Physics")]
        [Tooltip("Force to push plane forwards with")] public float thrust = 100f;
        [Tooltip("Pitch, Yaw, Roll")] public Vector3 turnTorque = new Vector3(90f, 25f, 45f);
        [Tooltip("Multiplier for all forces")] public float forceMult = 1000f;

        [Header("Input")]
        [SerializeField][Range(-1f, 1f)] private float pitch = 0f;
        [SerializeField][Range(-1f, 1f)] private float yaw = 0f;
        [SerializeField][Range(-1f, 1f)] private float roll = 0f;

        public float Pitch { get => pitch; set => pitch = Mathf.Clamp(value, -1f, 1f); }
        public float Yaw { get => yaw; set => yaw = Mathf.Clamp(value, -1f, 1f); }
        public float Roll { get => roll; set => roll = Mathf.Clamp(value, -1f, 1f); }

        private Rigidbody rigid;
        private NetworkObject networkObject;

        private bool rollOverride = false;
        private bool pitchOverride = false;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
            networkObject = GetComponent<NetworkObject>();
        }

        public void DisableMultiplayer()
        {
            if (networkObject)
            {
                networkObject.enabled = false;
            }
        }

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                enabled = false;
            }
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            yaw = Input.GetAxis("Horizontal");
            pitch = Input.GetAxis("Vertical");
            roll = Input.GetAxis("Horizontal");
        }

        private void FixedUpdate()
        {
            ApplyForces();
        }

        private void ApplyForces()
        {
            rigid.AddRelativeForce(Vector3.forward * thrust * forceMult, ForceMode.Force);
            rigid.AddRelativeTorque(new Vector3(turnTorque.x * pitch, turnTorque.y * yaw, -turnTorque.z * roll) * forceMult, ForceMode.Force);
        }
    }
}
