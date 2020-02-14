using UnityEngine;

public class PlayerPlanetInput : MonoBehaviour
{
    public PlanetController PlanetController { get; set; }
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && Time.timeScale != 0)
        {
            PlanetController.Shoot();
        }
    }
}