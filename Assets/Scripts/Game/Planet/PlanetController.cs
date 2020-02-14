using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetController : MonoBehaviour
{
    [SerializeField] private PlanetGenerator modelGenerator;
    [SerializeField] private WeaponHolderController weaponHolder;

    public bool IsControlledByPlayer = false;
    public Action<PlanetController> PlanetDestroyed;

    private Rigidbody2D rb;
    private Health health;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        health.OnDied += OnDied;
    }

    public void GenerateModel(float seed)
    {
        modelGenerator.GenerateModel(seed);

        var materialCopy = modelGenerator.GetComponent<MeshRenderer>().material;
        
        materialCopy.SetFloat("_RandomSeed", seed);

        modelGenerator.transform.localScale = Random.Range(1f, 2f) * Vector3.one;
    }

    private void OnDied()
    {
        Destroy(this.gameObject);
        PlanetDestroyed?.Invoke(this);
    }

    #region Weapon

    public void SetWeapon(Weapon weapon)
    {
        weaponHolder.SetWeapon(weapon);
    }

    public void LockWeapons(bool lockWeapons)
    {
        weaponHolder.LockWeapon(lockWeapons);
    }

    public void Shoot()
    {
        weaponHolder.Shoot();
    }

    #endregion
}