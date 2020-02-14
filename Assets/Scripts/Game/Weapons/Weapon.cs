using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void Shoot();
    public abstract bool IsReadyToShoot { get; }
}