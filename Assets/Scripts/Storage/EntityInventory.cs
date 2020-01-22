using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInventory : MonoBehaviour
{
    [SerializeField] public EntityType ToolItemInInventory { get; protected set; }
    [SerializeField] public EntityType ItemInInventory { get; protected set; }
    [SerializeField] public int ItemAmountInInventory { get; protected set; }

    public void SetTool(EntityType tool)
    {
        ToolItemInInventory = tool;
    }

    public void AddInventoryItem(EntityType itemType, int amount)
    {
        if(ItemAmountInInventory == 0 || ItemInInventory == EntityType.Empty)
        {
            ItemInInventory = itemType;
            ItemAmountInInventory = amount;
        }
    }

    public int RemoveInventoryItem(EntityType itemType, int amount)
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
