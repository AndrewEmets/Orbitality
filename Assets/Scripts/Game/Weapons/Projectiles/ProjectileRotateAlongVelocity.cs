using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRotateAlongVelocity : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var rbVelocity = rb.velocity;
        rb.SetRotation(Mathf.Atan2(rbVelocity.y, rbVelocity.x) * Mathf.Rad2Deg);
    }
}
