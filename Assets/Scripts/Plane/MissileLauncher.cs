using System.Collections.Generic;
using UnityEngine;

public class WaterMissileLauncher : MonoBehaviour
{
    [SerializeField] private GameObject waterMissilePrefab;
    [SerializeField] private float missileVelocityMultiplier = 3;
    [SerializeField] private ParticleSystem[] particleSystems;
    private Rigidbody _planeRb;
    
    private void Start()
    {
        _planeRb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (ParticleSystem ps in particleSystems)
            {
                Instantiate(ps, transform.transform);
            }
            LaunchWaterMissile();
        }
    }

    void LaunchWaterMissile()
    {
        GameObject missile = Instantiate(waterMissilePrefab, transform.position + Vector3.forward * 2, transform.rotation);
        missile.GetComponent<Rigidbody>().velocity = _planeRb.velocity * missileVelocityMultiplier;
    }
}