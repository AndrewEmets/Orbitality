using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarHUD : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private SpriteRenderer foreground;
    
    private void Start()
    {
        health.OnDamageDealt += OnDamageDealt;
    }

    private void LateUpdate()
    {
        transform.SetPositionAndRotation(health.transform.position + Vector3.down * 2.5f, Quaternion.identity);
    }

    private void OnDamageDealt()
    {
        var f = health.CurrentHealth / health.MaxHealth;
        f = Mathf.Clamp01(f);
        foreground.transform.localScale = new Vector3(10 * f, 1, 1);
    }
}
