using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    [SerializeField] private float amplitude;
    [SerializeField] private Vector3 scale = Vector3.one;
    private Vector3 angularVelocity;
    
    private void Awake()
    {
        angularVelocity = new Vector3(
            Random.Range(-amplitude,amplitude),
            Random.Range(-amplitude,amplitude),
            Random.Range(-amplitude,amplitude));
        
        angularVelocity.Scale(scale);
    }

    private void Update()
    {
        transform.Rotate(angularVelocity * Time.smoothDeltaTime);
    }
}