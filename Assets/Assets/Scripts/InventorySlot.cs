using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    private ItemTypes currentItem;
    private bool isEquipped = false;
    private inventorySystem inventory;

    void Start()
    {
        inventory = GetComponentInParent<inventorySystem>();
        if (icon != null) icon.enabled = false;
    }

    public bool IsEmpty()
    {
        return currentItem == null;
    }

    public void SetItem(ItemTypes item)
    {
        currentItem = item;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public ItemTypes GetItem()
    {
        return currentItem;
    }

    public void SetEquipped(bool equipped)
    {
        isEquipped = equipped;
        GetComponent<Image>().color = equipped ? Color.green : Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        inventory.ToggleEquip(this);
    }
}