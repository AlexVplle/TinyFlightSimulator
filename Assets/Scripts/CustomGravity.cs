using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    public float gravityMultiplier = 100f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        Vector3 gravity = gravityMultiplier * Physics.gravity * rb.mass;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }
}