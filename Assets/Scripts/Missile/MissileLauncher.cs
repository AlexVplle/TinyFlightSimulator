using SDD.Events;
using UnityEngine;

namespace Missile
{
    public class MissileLauncher: MonoBehaviour
    {
        public void Start()
        {
            
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                EventManager.Instance.Raise(new CreateNewMissileEvent {Transform = transform});
            }
        }
    }
}