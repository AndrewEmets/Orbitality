using UnityEngine;

public class PlanetRotatorAroundSun : MonoBehaviour
{
    private Transform sunTransform;
    private float radius;
    private float frequency;
    private float phase;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Transform sunTransform, float radius, float frequency, float phase)
    {
        this.sunTransform = sunTransform;
        this.radius = radius;
        this.frequency = frequency;
        this.phase = phase;

        rb.angularVelocity = (Random.value < 0.5 ? 1 : -1) * 100;
        rb.rotation = Random.Range(0f, 360f);
    }

    private void FixedUpdate()
    {
        var f = Time.fixedTime * frequency * Mathf.PI * 2 + phase;
        var pos = new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * radius + (Vector2) sunTransform.position;
        
        rb.MovePosition(pos);
        
    }
}