using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathfExt
{
    public static float Smoothstep(float a, float b, float x)
    {
        var t = (x - a) / (b - a);
        t = Mathf.Clamp01(t);
        t = (3 - 2 * t) * t * t;
        return t;
    }
}

public static class CollectionsExtensions
{
    public static T GetRandomElement<T>(this IList<T> collection)
    {
        return collection[Random.Range(0, collection.Count)];
    }
}