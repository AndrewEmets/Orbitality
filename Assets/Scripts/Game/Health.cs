using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsInvincible { get; set; }

    public Action OnDied;
    public Action OnDamageDealt;

    private bool isDead;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void DealDamage(float damage)
    {
        if (IsInvincible)
        {
            return;
        }
        
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;
        OnDamageDealt?.Invoke();

        if (currentHealth <= 0)
        {
            isDead = true;
            OnDied?.Invoke();
        }
    }
}