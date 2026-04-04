using UnityEngine;

public class ItemGet : MonoBehaviour
{
    public ItemTypes item;
    public inventorySystem inventory;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (inventory.AddItem(item))
            {
                Destroy(gameObject);
            }
        }
    }
}