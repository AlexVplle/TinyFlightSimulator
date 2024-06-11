using UnityEngine;

public class FireController : MonoBehaviour
{
    public float gravityMultiplier = 0.5f;
    
    private Rigidbody _rb;
    private bool _applyCustomGravity = true;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
    }

    void FixedUpdate()
    {
        if (_applyCustomGravity)
        {
            Vector3 customGravity = gravityMultiplier * Physics.gravity * _rb.mass;
            _rb.AddForce(customGravity, ForceMode.Acceleration);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        _applyCustomGravity = false;
        _rb.velocity = Vector3.zero;
        _rb.isKinematic = true;
    }
}
