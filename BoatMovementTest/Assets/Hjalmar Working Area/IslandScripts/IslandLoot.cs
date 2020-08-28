using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Loot")]
public class IslandLoot : ScriptableObject
{
    [SerializeField] private List<GameObject> lootPrefabs = new List<GameObject>();

    public GameObject GetRandomAsset()
    {
        int rand = Random.Range(0, lootPrefabs.Count);
        return lootPrefabs[rand];
    }
}
