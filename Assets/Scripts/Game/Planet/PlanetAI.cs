using System;
using System.Collections.Generic;
using UnityEngine;

public class PlanetAI : MonoBehaviour
{
    [SerializeField] private PlanetController planetController;
    [SerializeField] private WeaponHolderController weaponHolder;

    [SerializeField, Range(0,1)] private float aimAccuracy;

    private readonly List<PlanetController> otherPlanets = new List<PlanetController>();

    private void Awake()
    {
    }

    private void Start()
    {
        foreach (var planet in FindObjectsOfType<PlanetController>())
        {
            if (planet.gameObject == this.gameObject)
                continue;

            otherPlanets.Add(planet);
            planet.PlanetDestroyed += PlanetDestroyed;
        }
    }

    private void PlanetDestroyed(PlanetController p)
    {
        otherPlanets.Remove(p);
    }

    private void FixedUpdate()
    {
        if (!weaponHolder.IsReadyToShoot)
        {
            return;
        }

        PlanetController bestCandidateToShoot = null;
        
        var closestDot = Single.NegativeInfinity;
        var forward = transform.right;
        var thisPos = transform.position;
        
        for (var i = otherPlanets.Count - 1; i >= 0; i--)
        {
            var otherPlanet = otherPlanets[i];
            var delta = otherPlanet.transform.position - thisPos;

            var otherPlanetDirection = delta.normalized;

            var dot = Vector3.Dot(forward, otherPlanetDirection);

            if (dot > closestDot)
            {
                closestDot = dot;
                bestCandidateToShoot = otherPlanet;
            }
        }

        if (closestDot > aimAccuracy)
        {
            weaponHolder.Shoot();
        }
    }
}