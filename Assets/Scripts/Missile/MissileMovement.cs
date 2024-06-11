using System;
using SDD.Events;
using UnityEngine;

namespace  Missile
{
    public class MissileMovement : MonoBehaviour
    {
        [SerializeField] private float velocity;
        
        void Start()
        {
        }

        void Update()
        {
            CheckDestruction();
        }

        private void FixedUpdate()
        {
            transform.position += transform.forward * velocity;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Plane"))
            {
                return;
            }
            
            // bump into the ground
            EventManager.Instance.Raise(new DestroyMissileEvent {Missile = gameObject});

            if (other.CompareTag("Fireball"))
            {
                EventManager.Instance.Raise(new CreateSmokeEvent {Position = transform.position});
                EventManager.Instance.Raise(new DestroyFireballEvent {fireball = other.gameObject});
            }
        }

        private void CheckDestruction()
        {
            var distance = transform.position;
            var flightDistance = PlayerManager.playerReference.transform.position;
            if ((distance - flightDistance).magnitude >= 100)
            {
                EventManager.Instance.Raise(new DestroyMissileEvent {Missile = gameObject});
            }
        }
    }
}
