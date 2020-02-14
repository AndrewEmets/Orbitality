using UnityEngine;

public abstract class ProjectileDamageDealer : MonoBehaviour
{
    [SerializeField] protected float damage; 
    
    private ProjectileController projectileController;
    
    private void Awake()
    {
        projectileController = GetComponent<ProjectileController>();
    }

    private void Start()
    {
        projectileController.CollidedWithPlanet += OnCollidedWithPlanet;
    }

    protected abstract void OnCollidedWithPlanet(Health health);
}