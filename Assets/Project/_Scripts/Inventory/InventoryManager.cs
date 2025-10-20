using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Stats")]
    [SerializeField, Range (3f,5f)] private int rows = 5;
    [SerializeField, Range (3f,7f)] private int columns = 7;

    public Vector2Int GridSize => new Vector2Int(rows,columns);

    public static InventoryManager Instance;

    public ChestManager ChestManager;
    
    public GameObject SlotPrefab;
    public Transform GridLayoutGroup;

    public InventorySlot[] InventorySlots;
    public GameObject InventoryItemPrefab;


    [Header("UI - Grid Controls")]
    public TextMeshProUGUI rowsText;
    public TextMeshProUGUI columnsText;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreateSlots();
        UpdateUIText();
    }
    public void CreateSlots()
    {
        foreach(Transform child  in GridLayoutGroup)
        {
            Destroy(child.gameObject);
        }
        
        GridLayoutGroup grid = GridLayoutGroup.GetComponent<GridLayoutGroup>();
        grid.constraintCount = GridSize.x;
        int totalSlots = GridSize.x * GridSize.y;
        InventorySlots = new InventorySlot[totalSlots];

        for (int i = 0; i < totalSlots; i++)
        {
            GameObject newSlot = Instantiate(SlotPrefab, GridLayoutGroup);
            InventorySlot slot = newSlot.GetComponent<InventorySlot>();
            InventorySlots[i] = slot;
        }
    }

    public bool AddItem(ItemData itemData) 
    {
        for (int i = 0; i < InventorySlots.Length; i++) //Find an existing slot with the same item
        {
            InventorySlot slot = InventorySlots[i];
            InventoryItem ItemInInventory = slot.GetComponentInChildren<InventoryItem>();
            if (ItemInInventory != null && 
                ItemInInventory.ItemData == itemData && 
                ItemInInventory.Count <ItemInInventory.ItemData.MaxStack && 
                ItemInInventory.ItemData.isStacklable)
            {
                ItemInInventory.Count++;
                ItemInInventory.RefreshCount();
                return true;
            }

        }

        for (int i = 0; i < InventorySlots.Length; i++)    //Find an empty slot
        {
            InventorySlot slot = InventorySlots[i];
            InventoryItem itemInInventory = slot.GetComponentInChildren<InventoryItem>();
            if(itemInInventory == null)
            {
                SpawnNewItem(itemData, slot._itemTransform);
                return true;
            }
        }
        return false;
    }

    private void SpawnNewItem(ItemData itemData, Transform ItemTransform) // Create a new item in the empty slot
    {
        GameObject newItemGo = Instantiate(InventoryItemPrefab, ItemTransform.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(itemData);
    }

    internal void MoveItem(InventoryItem ItemtoMove, OwnerType TargetInventory)
    {
        InventorySlot[] TargetSlots = TargetInventory == OwnerType.Player ? InventorySlots : ChestManager.ChestSlots;
        Debug.Log(TargetSlots[0], TargetSlots[0].gameObject);

        foreach (var slot in TargetSlots)  //Search for an existing stack of the same item
        {
            InventoryItem IteminSlot = slot.GetComponentInChildren<InventoryItem>();
            
            if (IteminSlot != null &&
                IteminSlot.ItemData == ItemtoMove.ItemData &&
                IteminSlot.ItemData.isStacklable &&
                IteminSlot.Count < IteminSlot.ItemData.MaxStack) 
            {
                int remainingItemCount = IteminSlot.AddItemCount(ItemtoMove.Count);
                if (remainingItemCount > 0)
                {
                    ItemtoMove.Count = remainingItemCount;
                    ItemtoMove.RefreshCount();
                    return;
                }
                else
                {
                    ItemtoMove.CurrentOwner = TargetInventory;
                    Destroy(ItemtoMove.gameObject);
                    return;
                }
            }
        }

        foreach (var slot in TargetSlots)  // Search for an empty slot
        {
            InventoryItem IteminSlot = slot.GetComponentInChildren<InventoryItem>();
            if (IteminSlot == null)
            {
                if(ItemtoMove.CurrentOwner == OwnerType.Chest)
                {
                    ItemtoMove.CurrentOwner = OwnerType.Player;
                }
                else
                {
                    ItemtoMove.CurrentOwner = OwnerType.Chest;
                }
                
                ItemtoMove.ParentAfterDrag = slot._itemTransform;
                ItemtoMove.transform.SetParent(slot._itemTransform);
                return;
            }
        }
    }

    public void OnRowsSliderChanged(float value)
    {
        rows = Mathf.RoundToInt(value);
        UpdateUIText();
    }

    public void OnColumnsSliderChanged(float value)
    {
        columns = Mathf.RoundToInt(value);
        UpdateUIText();
    }

    private void UpdateUIText()
    {
        rowsText.text = $"{rows}";
        columnsText.text = $"{columns}";
    }
}
