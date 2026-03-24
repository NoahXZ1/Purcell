using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;

    [Header("UI")]
    [SerializeField] private GameObject promptRoot;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private InventoryUI inventoryUI;

    private readonly List<IInteractable> _nearby = new List<IInteractable>();   //store all items that are colliding with player
    private readonly List<ItemData> _inventory = new List<ItemData>();  //list of backpack(current items)

    private void Update()
    {
        CleanupNulls();  //remove all items that are collected at first
        IInteractable current = GetNearest();   //Get the nearest item to player when more than 1 item overlap with player's collider box. 

        if (current != null)
        {
            SetPrompt(true, current.PromptText);
            if (Input.GetKeyDown(interactKey))
                current.Interact(this);
        }
        else
        {
            SetPrompt(false);
        }
    }

    public void RegisterInteractable(IInteractable interactable)  //add item to interactable list
    {
        if (!_nearby.Contains(interactable))
            _nearby.Add(interactable);
    }

    public void UnregisterInteractable(IInteractable interactable) //remove item from interable list (so player cannot collect them when leaving)
    {
        _nearby.Remove(interactable);
    }

    public void AddItem(ItemData item) //add item to inventory list 
    {
        _inventory.Add(item);
        inventoryUI?.Refresh(_inventory);
    }

    public bool HasItem(ItemData item) => _inventory.Contains(item);  //(*encoming) interface to enable other script query whether player has some specific item. 

    public void RemoveItem(ItemData item)
    {
        _inventory.Remove(item);
        inventoryUI?.Refresh(_inventory);
    }

    // Returns the nearest interactable by world distance
    private IInteractable GetNearest()
    {
        IInteractable nearest = null;
        float minDist = float.MaxValue;

        foreach (var i in _nearby)  // calculate all interactable items' distance to player 
        {
            if (i is MonoBehaviour mb)
            {
                float dist = Vector2.Distance(transform.position, mb.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = i;
                }
            }
        }

        return nearest;
    }

    // Remove entries for GameObjects that have been destroyed
    private void CleanupNulls()
    {
        _nearby.RemoveAll(x => x is MonoBehaviour mb && mb == null);
    }

    private void SetPrompt(bool visible, string text = "")  //set collectable item's prompt visiable on screen. 
    {
        if (promptRoot) promptRoot.SetActive(visible);
        if (promptText) promptText.text = text;
    }
}
