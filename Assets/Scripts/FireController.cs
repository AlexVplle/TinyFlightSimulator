using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField] private float velocity;
    private bool _applyGravity = true;
    
    private void Gravity()
    {
        transform.position += 20 * Time.deltaTime * -1 * transform.up;
    }

    void Update()
    {
        if (!_applyGravity)
        {
            return;
        }
        Gravity();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("boulette de merde qui rentre dans le sol");
        _applyGravity = false;
    }
}
