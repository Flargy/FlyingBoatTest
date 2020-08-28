using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tesing : MonoBehaviour
{
    [SerializeField] public List<Transform> spawnLocation = new List<Transform>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (spawnLocation[0] != null)
            {
                IslandSpawner.CreateAnIsland(spawnLocation[0]);
                spawnLocation.Remove(spawnLocation[0]);
            }
        }
    }
}
