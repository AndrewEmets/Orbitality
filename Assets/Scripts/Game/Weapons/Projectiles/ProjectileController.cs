using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    
    public float StartForce { get; set; }

    public Action<Health> CollidedWithPlanet;
    
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Push(StartForce);

        //StartCoroutine(StateChangeEvent());
    }

    private IEnumerator StateChangeEvent()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Collider2D>().enabled = true;
    }

    public void Push(float force)
    {
        rb.AddRelativeForce(Vector2.right * force, ForceMode2D.Impulse);
    }

    public void SetPositionAndRotation(Vector3 transformPosition, Quaternion transformRotation)
    {
        rb.MovePosition(transformPosition);
        rb.MoveRotation(transformRotation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((layerMask & (1 << other.gameObject.layer)) != 0)
        {
            var health = other.GetComponent<Health>();

            if (health != null)
            {
                CollidedWithPlanet?.Invoke(health);
                Destroy(this.gameObject);
            }
        }
    }
}
