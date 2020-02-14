using UnityEngine;

public class WeaponHolderController : MonoBehaviour
{
    private PlanetController parentPlanetController;
    private Weapon weapon;

    public bool IsReadyToShoot => !isLocked && (weapon?.IsReadyToShoot ?? false);

    private void Awake()
    {
        parentPlanetController = GetComponentInParent<PlanetController>();
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        this.weapon.transform.SetParent(this.transform);
        this.weapon.transform.localPosition = Vector3.zero;
        this.weapon.transform.localRotation = Quaternion.identity;
    }
    
    private bool shouldShoot;
    private bool isLocked;

    public void Shoot()
    {
        if (isLocked) return;
        
        weapon?.Shoot();
    }

    public void LockWeapon(bool lockWeapons)
    {
        isLocked = lockWeapons;
    }
}
