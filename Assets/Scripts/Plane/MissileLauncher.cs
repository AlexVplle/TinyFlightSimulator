using SDD.Events;
using UnityEngine;

namespace Plane
{
    public class MissileLauncher: MonoBehaviour
    {
        [SerializeField] private float _missileVelocityMultiplier;
        private Rigidbody _planeRb;

        private void Start()
        {
            _planeRb = GetComponentInParent<Rigidbody>();
        }
        
        private void Update()
        {
            Debug.Log(_planeRb.velocity * _missileVelocityMultiplier);
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                EventManager.Instance.Raise(new CreateNewMissileEvent
                {
                    Transform = transform,
                    Velocity = _planeRb.velocity * _missileVelocityMultiplier
                });
            }
        }
    }
}