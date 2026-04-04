using UnityEngine;

/// <summary>
/// Portal interaction: Human form only. Press E to move
/// </summary>
public class Portal : MonoBehaviour, IInteractable
{
    [Header("Portal Settings")]
    [Tooltip("The other Portal object this one connects to.")]
    [SerializeField] private Portal linkedPortal;  //set the linked portal in Inspector.

    public string PromptText => "Press E to enter the portal";  //prompts when player touch the portal in human form. 

    // Cached reference to the player currently in range (null when nobody is near).
    private PlayerInteraction _playerInteraction;
    private PlayerMovement    _playerMovement;

    private void OnTriggerEnter2D(Collider2D other)  //when collider box overlaped
    {
        // Support both root collider and child colliders (human/cat).
        var movement = other.GetComponentInParent<PlayerMovement>();
        if (movement == null) return;

        // Cat form: do nothing (no prompt, no teleport).
        if (movement.currentForm != PlayerMovement.PlayerForm.Human) return;  //if the player current form is cat, disable interaction. 

        var interaction = movement.GetComponent<PlayerInteraction>();  //if player is human form but without pressing E, no interaction happened. 
        if (interaction == null) return;

        _playerMovement    = movement;
        _playerInteraction = interaction;
        _playerInteraction.RegisterInteractable(this);  //add the portal into the registered interactable list
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Only react to the collider that originally registered.
        var movement = other.GetComponentInParent<PlayerMovement>();
        if (movement == null || movement != _playerMovement) return;

        Unregister();
    }

    public void Interact(PlayerInteraction player)   //function is contiously called in the Update() in PlayerInteraction.cs
    {
        if (linkedPortal == null)
        { //show warning message if no linked portal in other side. 
            Debug.LogWarning($"[Portal] {name} has no linked portal assigned.", this);
            return;
        }

        // Teleport the player root to the linked portal's position.
        player.transform.position = linkedPortal.transform.position;
    }

    private void Unregister()
    {
        _playerInteraction?.UnregisterInteractable(this);
        _playerInteraction = null;
        _playerMovement    = null;
    }

    private void OnDisable()
    {
        // Safety cleanup if the portal GameObject is disabled mid-interaction.
        Unregister();
    }
}
