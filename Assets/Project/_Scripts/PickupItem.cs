using UnityEngine;
using UnityEngine.EventSystems;

public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData _itemData;

    public void Interact()
    {
        InventoryManager.Instance.AddItem(_itemData);
    }

}
