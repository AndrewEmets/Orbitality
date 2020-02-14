using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Random = UnityEngine.Random;

public struct StartGameParameters
{
    public int playersCount;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlanetController planetControllerPrefab;
    [SerializeField] private Transform sunTransform;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private GameObject playerIndicator_prefab;

    public Action<GameResult> GameFinished;

    private readonly List<PlanetController> planets = new List<PlanetController>();
    
    public void StartGame(StartGameParameters parameters)
    {
        GeneratePlanets(parameters.playersCount);
    }

    private void GeneratePlanets(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var planet = Instantiate(planetControllerPrefab);

            var planetRotator = planet.GetComponent<PlanetRotatorAroundSun>();
            planetRotator.Init(sunTransform,
                radius: (i + 1) * 10,
                frequency: Random.Range(0.25f, 1f) * Mathf.Sign(Random.value - 0.5f) / ((i + 1) * 10),
                phase: Random.value * Mathf.PI * 2);

            planet.GenerateModel(Random.value * 10000f);

            planet.SetWeapon(Instantiate(weapons.GetRandomElement()));

            planets.Add(planet);

            if (i == 0)
            {
                Instantiate(playerIndicator_prefab, planet.transform.position, Quaternion.identity, planet.transform);
                
                planet.IsControlledByPlayer = true;
                
                var input = planet.gameObject.AddComponent<PlayerPlanetInput>();
                input.PlanetController = planet;
                Destroy(planet.GetComponent<PlanetAI>());
            }
            
            planet.PlanetDestroyed += PlanetDestroyed;
        }
    }

    private void PlanetDestroyed(PlanetController p)
    {
        planets.Remove(p);

        if (p.IsControlledByPlayer)
        {
            FinishGame(GameResult.Lose);
        }
        else
        {
            if (planets.Count == 1)
            {
                FinishGame(GameResult.Win);
            }
        }
    }

    public enum GameResult
    {
        Win, Lose
    }

    private void FinishGame(GameResult gameResult)
    {
        foreach (var p in planets)
        {
            var planetAi = p.GetComponent<PlanetAI>();
            if (planetAi != null)
            {
                planetAi.enabled = false;
            }

            p.GetComponent<Health>().IsInvincible = true;
            p.LockWeapons(true);
        }
        
        GameFinished?.Invoke(gameResult);
    }
}
