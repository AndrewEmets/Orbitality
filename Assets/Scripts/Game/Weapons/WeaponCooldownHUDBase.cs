using UnityEngine;

public abstract class WeaponCooldownHUDBase : MonoBehaviour
{
    [SerializeField] private GameObject weaponGameObject;
    
    private void Awake()
    {
        var weapon = weaponGameObject.GetComponent<IHasCooldown>();
        
        weapon.CooldownProgressUpdated += CooldownProgressUpdated;
    }

    protected abstract void CooldownProgressUpdated(float progress);
}