using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemData")]
public class ItemData : ScriptableObject
{
    static private int MyID = 0;
    [SerializeField] public string itemDescription;
    [SerializeField] public Sprite itemImage;
    [SerializeField] public int maxCapacity;
    [SerializeField] public int itemID = MyID++;
}
