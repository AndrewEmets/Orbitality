using System;
using System.Collections;
using UnityEngine;

public interface IHasCooldown
{
    Action<float> CooldownProgressUpdated { get; set; }
}

public class CannonWeapon : Weapon, IHasCooldown
{
    [SerializeField] private ProjectileController projectilePrefab;
    [SerializeField] private float shootForce;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float coolDown;

    public Action<float> CooldownProgressUpdated { get; set; }
    public override bool IsReadyToShoot => isReadyToShoot;

    private bool isReadyToShoot = true;
    private float cooldownProgress;

    public override void Shoot()
    {
        if (!isReadyToShoot)
        {
            return;
        }
        
        var proj = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        proj.StartForce = shootForce;
        isReadyToShoot = false;
        
        StartCoroutine(StartWaitingCooldown());
    }


    private IEnumerator StartWaitingCooldown()
    {
        for (float t = 0; t <= 1; t += Time.deltaTime / coolDown)
        {
            cooldownProgress = t;
            CooldownProgressUpdated?.Invoke(cooldownProgress);
            
            yield return null;
        }

        cooldownProgress = 1;
        CooldownProgressUpdated?.Invoke(cooldownProgress);
        
        isReadyToShoot = true;
    }
}