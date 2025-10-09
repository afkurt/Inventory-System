using UnityEngine;

public class TestButtons : MonoBehaviour
{
    
    public ItemData[] itemDatas;

    public void PickupItem(int id)
    {
        bool result = InventoryManager.Instance.AddItem(itemDatas[id]);
        Debug.Log(result ? "Item Generated " : "Inventory is full");
        
    }
}
