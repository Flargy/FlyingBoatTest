using UnityEngine.UI;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private GameObject myGameObject;
    [SerializeField] private Image myImageDisplay;
    [SerializeField] private Text myAmmount;
    [SerializeField] private ItemData myItemData;
    [SerializeField] private InventoryGuide playerInventory;

    [SerializeField] private int currentAmmount = 0;
    public int CurrentAmmount { get { return currentAmmount; } set { currentAmmount = value; } }

    public int MyItemID { get { return myItemData.itemID; } }
    public GameObject fisk { get; set; }
    public GameObject MyGameObject { get { return myGameObject; } set { myGameObject = value; } }
    public ItemData MyItemData { get { return myItemData; } set { myItemData = value; } }

    public void Awake()
    {
        if (myGameObject == null)
            myGameObject = gameObject;
        if(myImageDisplay == null)
            myImageDisplay = GetComponent<Image>();
        myAmmount.text = "" +  CurrentAmmount;
        myImageDisplay.sprite = MyItemData.itemImage;
    }

    public bool CanHoldMore()
    {
        return CurrentAmmount < myItemData.maxCapacity;
    }

    public void MergeIntoOne(InventoryItem item)
    {
        Debug.Log("I am now adding " + item.CurrentAmmount);
        currentAmmount += item.CurrentAmmount;
        if(currentAmmount > MyItemData.maxCapacity)
        {
            Debug.Log("Can't hold more. I'm sending request to find create new");
            int extendedAmmount = currentAmmount - MyItemData.maxCapacity;
            currentAmmount -= extendedAmmount;
            item.CurrentAmmount = extendedAmmount;
            InventoryGuide.AddItemToInventory(item);
        }
        Debug.Log("I am now updated and now hold " + currentAmmount);
        myAmmount.text = "" + currentAmmount;
    }

    public void OnMouseDown()
    {
        Debug.Log("This doesn't happen");
        InventoryGuide.AddItemToInventory(this);
    }
}
