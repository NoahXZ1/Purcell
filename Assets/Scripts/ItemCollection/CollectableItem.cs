using UnityEngine;

// Press-E-to-collect item. Attach to an item GameObject that has a trigger Collider2D.
public class CollectableItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;

    public string PromptText => $"Press E to pick up {itemData.itemName}";  //property accessor to get the value of itemData (as a property of PromptText)

    public void Interact(PlayerInteraction player) //Add keys to backpack and remove it if interactable. 
    {
        player.AddItem(itemData);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)  //Set the item as interactable when player touch it
    {
        var player = other.GetComponent<PlayerInteraction>()
                  ?? other.GetComponentInParent<PlayerInteraction>(); //If both variables are null, then player = null
        player?.RegisterInteractable(this);  //only call the function when player != null. 
    }

    private void OnTriggerExit2D(Collider2D other)  //Disable the interaction with item when player leave it. 
    {
        var player = other.GetComponent<PlayerInteraction>()
                  ?? other.GetComponentInParent<PlayerInteraction>();
        player?.UnregisterInteractable(this);
    }
}
