using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationNeedle : MonoBehaviour
{
    [SerializeField]
    private Transform planeTransform;

    void Update()
    {
        if (planeTransform != null)
        {
            Vector3 euler = transform.rotation.eulerAngles;
            euler.z = planeTransform.rotation.eulerAngles.z;
            transform.rotation = Quaternion.Euler(euler);
        }
    }
}
