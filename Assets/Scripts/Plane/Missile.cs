using SDD.Events;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float velocity;
    [SerializeField] private GameObject flight;
    private void Update()
    {
        MoveMissile();
    }

    private void MoveMissile()
    {
        transform.position += velocity * Time.deltaTime * transform.forward;
    }

    private void CheckForMissileTooFar()
    {
        if ((flight.GetComponent<Transform>().position - transform.position).magnitude <= 100)
        {
            return;
        }
        EventManager.Instance.Raise(new DestroyMissileEvent
        {
            Missile = gameObject
        });
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plane"))
        {
            return;
        }
        EventManager.Instance.Raise(new DestroyMissileEvent
        {
            Missile = gameObject
        });
    }
}
