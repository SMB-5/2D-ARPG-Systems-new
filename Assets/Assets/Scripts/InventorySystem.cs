using System.Collections.Generic;
using UnityEngine;

public class inventorySystem : MonoBehaviour
{
    public List<InventorySlot> slots;
    public InventorySlot equippedSlot;

    void Start()
    {
        slots.AddRange(GetComponentsInChildren<InventorySlot>());
    }

    public bool AddItem(ItemTypes item)
    {
        foreach (var slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.SetItem(item);
                return true;
            }
        }

        Debug.Log("Inventory full");
        return false;
    }

    public void ToggleEquip(InventorySlot clickedSlot)
    {
        if (clickedSlot.IsEmpty()) return;

        ItemTypes item = clickedSlot.GetItem();
        if (item.itemType != ItemType.Weapon) return;

        //unequip if it's the same weapon
        if (equippedSlot == clickedSlot)
        {
            clickedSlot.SetEquipped(false);
            equippedSlot = null;
            Debug.Log("Weapon unequipped");
            return;
        }

        //unequip the previous weapon
        if (equippedSlot != null)
        {
            equippedSlot.SetEquipped(false);
        }

        //equip new weapon
        equippedSlot = clickedSlot;
        clickedSlot.SetEquipped(true);
        Debug.Log("Equipped: " + item.itemName);
    }

    public bool IsEquipped(ItemTypes item)
    {
        if (equippedSlot == null) return false;
        return equippedSlot.GetItem().itemName == item.itemName;
    }
}