using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    // Simple text display. Replace or extend with slot-based icon UI in the future.
    [SerializeField] private TMP_Text itemListText;

    public void Refresh(List<ItemData> inventory)
    {
        if (itemListText == null) return;

        if (inventory.Count == 0)
        {
            itemListText.text = "";
            return;
        }
        //Above will return text as empty when there are no items in the inventory list. 

        var entries = inventory
            .GroupBy(i => i.itemName) //Linq methods, divided all items in inventorylist by their name.
            .Select(g => g.Count() > 1 ? $"{g.Key} x{g.Count()}" : g.Key);   //When amount of a kind of item >= 2, show the count " x*", or just show item name. 

        itemListText.text = "Items: " + string.Join(", ", entries);
    }
}
