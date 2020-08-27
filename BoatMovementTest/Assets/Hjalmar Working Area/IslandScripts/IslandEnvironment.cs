using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Environment")]
public class IslandEnvironment : ScriptableObject
{
    [SerializeField] private List<GameObject> environmentalPrefabs = new List<GameObject>();
    [SerializeField] private Material baseIslandMaterial;

    public GameObject GetRandomAsset()
    {
        int rand = Random.Range(0, environmentalPrefabs.Count);
        return environmentalPrefabs[rand];
    }

    public Material GetIslandMaterial()
    {
        return baseIslandMaterial;
    }
}
