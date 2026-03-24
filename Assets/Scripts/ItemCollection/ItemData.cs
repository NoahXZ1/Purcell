using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;    //Value of this will get by accessor of PromptText in CollectableItem.cs to show the name of the key. 
    public Sprite icon; // reserved for future slot-based UI
}
