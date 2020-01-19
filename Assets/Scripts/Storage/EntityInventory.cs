using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInventory : MonoBehaviour
{
    [SerializeField] public StaticEntityType ToolItemInInventory { get; protected set; }
    [SerializeField] public StaticEntityType ItemInInventory { get; protected set; }
    [SerializeField] public int ItemAmountInInventory { get; protected set; }

    public void SetTool(StaticEntityType tool)
    {
        ToolItemInInventory = tool;
    }

    public void AddInventoryItem(StaticEntityType itemType, int amount)
    {
        if(ItemAmountInInventory == 0 || ItemInInventory == StaticEntityType.Empty)
        {
            ItemInInventory = itemType;
            ItemAmountInInventory = amount;
        }
    }

    public int RemoveInventoryItem(StaticEntityType itemType, int amount)
    {
        if (ItemInInventory == itemType)
        {
            int returnAmount = amount;
            amount = 0;
            return returnAmount;
        }
        return 0;
    }
}
