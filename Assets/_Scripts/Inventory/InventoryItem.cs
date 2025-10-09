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

    

    [HideInInspector] public ItemData ItemData;
    [SerializeField] public int Count = 1;
    [HideInInspector] public Transform ParentAfterDrag;

    public void InitialiseItem(ItemData newItem)    //scripte item datay� ver
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
        Debug.Log(" Eklendi");
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
        Debug.Log("Begin Drag");
        ParentAfterDrag = transform.parent;
        transform.SetParent(transform.root);   // Parenti canvas yap
        transform.SetAsLastSibling(); // hierarchy de en alta getir
        image.raycastTarget = false;  // item tutulduktan sonra mouse ile alg�lanmas�n
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Ended");
        transform.SetParent(ParentAfterDrag);  // Eski parenti getir
        image.raycastTarget = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) return;

        if(InventoryManager.Instance.ChestManager.isChestOpen)   // chest a��ksa
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.ItemMoveClip);
            if (CurrentOwner == OwnerType.Player) // oyuncudan chest'e g�nder
            {
                InventoryManager.Instance.MoveItem(this, OwnerType.Chest);
                Debug.Log(" Oyuncudan cheste g�nderiyorum");
                return;
            }
            else if (CurrentOwner == OwnerType.Chest)  // chestten oyuncuya g�nder
            {
                InventoryManager.Instance.MoveItem(this, OwnerType.Player);
                Debug.Log(" Chestten Oyuncuya g�nderiyorum");
                return;
            }
        }
        else  //chest kapal�ysa
        {
            if (ItemData.isStacklable)
            {
                AudioManager.Instance.PlaySound(AudioManager.Instance.ItemUseClip);
                Count--;
                RefreshCount();
                if (Count == 0) Destroy(gameObject);
            }
                

        }

    }

    private void UseItem()
    {
        Debug.Log("item used");
    }

    
}
