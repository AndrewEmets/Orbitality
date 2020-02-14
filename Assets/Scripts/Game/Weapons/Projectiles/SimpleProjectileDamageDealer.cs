using System;

public class SimpleProjectileDamageDealer : ProjectileDamageDealer
{
    protected override void OnCollidedWithPlanet(Health health)
    {
        health.DealDamage(damage);
    }
}