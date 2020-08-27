using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class IslandBase : MonoBehaviour
{
    [Header("The Islands Specifications")]
    [SerializeField] private IslandEnvironment myEnvironment;
    [SerializeField] private IslandLoot myIslandLoot;

    [Header("Lists for the spawnPoints")]
    [SerializeField] private List<Transform> environmentalTransforms = new List<Transform>();
    [SerializeField] private List<Transform> lootTransforms = new List<Transform>();
    [SerializeField] private List<MeshRenderer> allMyRenderers = new List<MeshRenderer>();

    private float ammountOfItems = 0;

    public void AssingIslandSpecifications(IslandEnvironment environment, IslandLoot loot)
    {
        myEnvironment = environment;
        myIslandLoot = loot;
        ammountOfItems = Random.Range(1, lootTransforms.Count);
        StartCoroutine(SpawnAllMyAssets());
    }

    IEnumerator SpawnAllMyAssets()
    {
        float spawnChans = 0;
        foreach(Transform environTrans in environmentalTransforms)
        {
            spawnChans = Random.Range(1, 101);

            if(spawnChans > 30)
            {
                GameObject environmentAsset = Instantiate(myEnvironment.GetRandomAsset(), environTrans);
            }
            yield return null;
        }

        int spawnedItems = 0;
        while(spawnedItems < ammountOfItems)
        {
            foreach (Transform lootTrans in lootTransforms)
            {
                spawnChans = Random.Range(1, 101);

                if (spawnChans > 40 && spawnedItems < ammountOfItems)
                {
                    GameObject lootAsset = Instantiate(myIslandLoot.GetRandomAsset(), lootTrans);
                    spawnedItems++;
                }
                yield return null;
            }
        }

        Material newMAt = myEnvironment.GetIslandMaterial();
        foreach (MeshRenderer renderer in allMyRenderers)
        {
            renderer.sharedMaterial = newMAt;
        }

        yield return null;        
    }
}
