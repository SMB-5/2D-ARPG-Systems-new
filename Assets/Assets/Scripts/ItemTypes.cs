using UnityEngine;

public enum ItemType
{
    Weapon
}

[System.Serializable]
public class ItemTypes
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
}