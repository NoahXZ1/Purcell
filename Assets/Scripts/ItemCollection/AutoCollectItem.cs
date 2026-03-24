using UnityEngine;

// Auto-collect item (e.g. energy orb). Collected immediately on trigger overlap, no key press needed.
// Attach to an item GameObject that has a trigger Collider2D.
public class AutoCollectItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerInteraction>()
                  ?? other.GetComponentInParent<PlayerInteraction>();
        if (player == null) return;  //Item will not be collected if collider box is not player.

        player.AddItem(itemData);  //Add item to player's backpack and remove item. 
        Destroy(gameObject);
    }
}
