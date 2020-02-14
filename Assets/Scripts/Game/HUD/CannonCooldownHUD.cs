using UnityEngine;

public class CannonCooldownHUD : WeaponCooldownHUDBase
{
    [SerializeField] private Transform indicator;
    
    protected override void CooldownProgressUpdated(float progress)
    {
        indicator.transform.localScale = new Vector3(1 - progress, 1, 1);
    }
}