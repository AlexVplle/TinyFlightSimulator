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
