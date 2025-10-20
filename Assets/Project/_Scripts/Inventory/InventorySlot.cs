using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Transform _itemTransform;
    [SerializeField] public GameObject SelectedUI;

    public OwnerType CurrentOwner;


    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        if (_itemTransform.childCount == 0)  // slot boþsa
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.ItemMoveClip);
            droppedItem.ParentAfterDrag = _itemTransform;
            if (CurrentOwner != droppedItem.CurrentOwner)
            {
                droppedItem.CurrentOwner = CurrentOwner;
            }

        }
        else
        {
            InventoryItem existingItem = _itemTransform.GetComponentInChildren<InventoryItem>();

            if (existingItem.ItemData == droppedItem.ItemData && existingItem.ItemData.isStacklable) //ayný itemse
            {
                int remainingItemCount = existingItem.AddItemCount(droppedItem.Count);
                AudioManager.Instance.PlaySound(AudioManager.Instance.ItemMoveClip);
                if (remainingItemCount > 0)
                {
                    droppedItem.Count = remainingItemCount;
                    droppedItem.RefreshCount();

                }
                else
                {
                    Destroy(droppedItem.gameObject);
                }
            }
            else   // farklý itemse 
            {
                Transform tempParent = droppedItem.ParentAfterDrag;
                AudioManager.Instance.PlaySound(AudioManager.Instance.ItemMoveClip);
                existingItem.transform.SetParent(tempParent);
                existingItem.ParentAfterDrag = tempParent;

                droppedItem.ParentAfterDrag = _itemTransform;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_itemTransform.childCount != 0)
        {
            SelectedUI.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SelectedUI.gameObject.SetActive(false);
    }
}
