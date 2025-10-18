using UnityEngine;




[CreateAssetMenu(fileName = "NewItem", menuName ="Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Only Gameplay")]
    public ItemType itemtype;


    [Header("Only UI")]
    public bool isStacklable = true;
    public int MaxStack = 20;

    [Header("Both")]
    public Sprite icon;

    public enum ItemType
    {
        Weapon,
        Supply
    }
}
