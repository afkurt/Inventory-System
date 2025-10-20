using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("UI")]
    public Image image;
    public TextMeshProUGUI CountText;

    public OwnerType CurrentOwner;
    public InventoryManager InventoryManager;
    public AudioManager AudioManager;

    [HideInInspector] public ItemData ItemData;
    [SerializeField] public int Count = 1;
    [HideInInspector] public Transform ParentAfterDrag;

    private void Start()
    {
        InventoryManager = InventoryManager.Instance;
        AudioManager = AudioManager.Instance;
    }
    public void InitialiseItem(ItemData newItem)    //Assign the item data to this script
    {
        ItemData = newItem;
        image.sprite = newItem.icon;
        RefreshCount();
    }

    public int AddItemCount(int count)
    {
        Count += count;
        if(ItemData.MaxStack <  Count)
        {
            int remainingOnHand = Count - ItemData.MaxStack;
            Count = ItemData.MaxStack;
            RefreshCount();
            return remainingOnHand;
        }
        RefreshCount();
        return 0;
        
    }

    public void RefreshCount()
    {
        CountText.text = Count.ToString();
        bool isActive = Count > 1;
        CountText.gameObject.SetActive(isActive);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        ParentAfterDrag = transform.parent;
        transform.SetParent(transform.root);   // Set parent to Canvas
        transform.SetAsLastSibling(); // Bring this item to the bottom of the hierarchy
        image.raycastTarget = false;  // Disable raycast so it doesn't block other UI while dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(ParentAfterDrag);  // Restore original parent
        image.raycastTarget = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) return;

        if(InventoryManager.ChestManager.isChestOpen)   // If chest is open
        {
            AudioManager.PlaySound(AudioManager.ItemMoveClip);
            if (CurrentOwner == OwnerType.Player) // Move item from Player to Chest
            {
                InventoryManager.MoveItem(this, OwnerType.Chest);
                return;
            }
            else if (CurrentOwner == OwnerType.Chest)  // Move item from Chest to Player
            {
                InventoryManager.MoveItem(this, OwnerType.Player);
                return;
            }
        }
        else  //If chest is closed
        {
            if (ItemData.isStacklable)
            {
                AudioManager.PlaySound(AudioManager.ItemUseClip);
                Count--;
                RefreshCount();
                if (Count == 0) Destroy(gameObject);
            }
                

        }

    }

    
}
