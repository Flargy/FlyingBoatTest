using System.Collections.Generic;
using UnityEngine;

public class IslandSpawner : MonoBehaviour
{
    private static IslandSpawner instance;

    [SerializeField] public List<IslandEnvironment> allEnvironments = new List<IslandEnvironment>();
    [SerializeField] public List<IslandLoot> allLoots = new List<IslandLoot>();
    [SerializeField] public List<GameObject> allIslands = new List<GameObject>();


    public static IslandSpawner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<IslandSpawner>();
            }
            if (instance == null)
            {
                Debug.LogError("Singleton<" + typeof(IslandSpawner) + "> instance has been not found.");
            }
            return instance;
        }
    }

    static public void CreateAnIsland(Transform location)
    {
        if(Instance == null)
        {
            Debug.Log("your instance is null");
            return;
        }
        int rand = Random.Range(0, Instance.allIslands.Count);
        GameObject newIsland = Instantiate(Instance.allIslands[rand]);
        newIsland.transform.position = location.position;
        newIsland.transform.rotation = location.rotation;

        IslandBase islandBase = newIsland.GetComponent<IslandBase>();
        rand = Random.Range(0, Instance.allEnvironments.Count);
        int temporaryRand = Random.Range(0, Instance.allLoots.Count);

        islandBase.AssingIslandSpecifications(Instance.allEnvironments[rand], Instance.allLoots[temporaryRand]);
    }
}
