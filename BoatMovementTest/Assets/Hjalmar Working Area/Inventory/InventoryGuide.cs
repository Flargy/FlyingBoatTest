using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryGuide : MonoBehaviour
{
    private static InventoryGuide myInstance;
    [SerializeField] private GameObject inventorySlotsUI;
    [SerializeField] private List<Transform> bagSlots = new List<Transform>();
    private List<Transform> usedBagslots = new List<Transform>();
    [SerializeField] private GameObject item;
    private Dictionary<Transform, InventoryItem> myInventory = new Dictionary<Transform, InventoryItem>();

    private bool backpackOpen = false;

    public static InventoryGuide Instance
    {
        get
        {
            if (myInstance == null)
            {
                myInstance = FindObjectOfType<InventoryGuide>();
            }
            if (myInstance == null)
            {
                Debug.LogError("Singleton<" + typeof(InventoryGuide) + "> instance has been not found.");
            }
            return myInstance;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            backpackOpen = !backpackOpen;
            inventorySlotsUI.SetActive(backpackOpen);
        }
        if (Input.GetKeyDown(KeyCode.Space) && backpackOpen)
        {
            item.GetComponent<InventoryItem>().CurrentAmmount = Random.Range(0, 11);
            AddItemToInventory(item.GetComponent<InventoryItem>());
        }
    }

    public static void AddItemToInventory(InventoryItem newItem)
    {
        if (Instance.myInventory.ContainsValue(newItem))
        {
            bool foundButNotCreated = false;
            foreach (KeyValuePair<Transform, InventoryItem> pair in Instance.myInventory)
            {
                if(pair.Value == newItem)
                {
                    Debug.Log("I found the same object");
                    if (pair.Value.CanHoldMore())
                    {
                        Debug.Log("I could still add more");
                        pair.Value.MergeIntoOne(newItem);
                        foundButNotCreated = false;
                    }
                    else
                    {
                        Debug.Log("It was already full");
                        foundButNotCreated = true;
                    }
                }
            }
            if (foundButNotCreated)
            {
                Debug.Log("Everyone was full so I created a new one");
                Instance.AddAndCreate(newItem);
            }
        }
        else if (Instance.bagSlots.Count > 0)
        {
            Debug.Log("Yea I did not exist so Im new");
            Instance.AddAndCreate(newItem);
        }
    }

    public void AddAndCreate(InventoryItem item)
    {
        myInventory.Add(bagSlots[0], item);
        GameObject addItem = Instantiate(item.MyGameObject, bagSlots[0]);
        usedBagslots.Add(bagSlots[0]);
        bagSlots.RemoveAt(0);
    }

    public void OnMouseOver()
    {
        Debug.Log("This happens");
    }
}
