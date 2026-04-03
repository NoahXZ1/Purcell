using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Gate that physically blocks Cat form but lets Human form pass through.
/// Shows a one-time notification on first contact (any form).
/// </summary>
public class Gate : MonoBehaviour
{
    [Header("Gate Collider")]
    [Tooltip("The non-trigger Collider2D that physically blocks the Cat form.")]
    [SerializeField] private Collider2D solidCollider;  //set in Inspector

    [Header("One-time Notification")]
    [SerializeField] private GameObject notificationPanel;  //prompt panel, set in Inspector
    [SerializeField] private TMP_Text   notificationText;  //prompt text
    [SerializeField] private float      displayDuration = 4f;  //the time that the prompt been showed after triggered

    private const string Message =
        "the gate is imbuing spiritual energy, you can only pass in your spirit form";  //the content of prompt

    private bool           _promptShown;
    private PlayerMovement _playerMovement;
    private int            _collidersInZone;   // counts active colliders of the player inside the trigger
    private Coroutine      _hideRoutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var movement = other.GetComponentInParent<PlayerMovement>();
        if (movement == null) return;

        _collidersInZone++;

        if (_playerMovement != movement)
        {
            // Safety: unsubscribe from any previous player reference.
            if (_playerMovement != null)   //clear the player form recorded in last time's colliding
                _playerMovement.OnFormChanged -= OnPlayerFormChanged;

            _playerMovement = movement;
            _playerMovement.OnFormChanged += OnPlayerFormChanged;

            // One-time prompt on first ever contact.
            if (!_promptShown)
            {
                _promptShown = true;
                ShowNotification();
            }
        }

        UpdateGateCollider(movement.currentForm);  //to determine whether the solid collider box is enabled or not. 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var movement = other.GetComponentInParent<PlayerMovement>();
        if (movement == null || movement != _playerMovement) return;

        _collidersInZone--;

        // Only truly "left" when no collider of this player remains inside.
        // During a form switch, Exit fires for the old collider then Enter fires
        // for the new one, so _collidersInZone briefly hits 0 and climbs back.
        if (_collidersInZone <= 0)
        {
            _collidersInZone = 0;
            _playerMovement.OnFormChanged -= OnPlayerFormChanged;
            _playerMovement = null;

            // Player has fully left — restore the solid wall.
            if (solidCollider) solidCollider.enabled = true;
        }
    }

    // is always updated ahead of the collider swap.

    //when Playermovement send a new event (like change the form), this method detects and call the updateGateCollider()
    //This is actually a listener of OnFormChanged
    private void OnPlayerFormChanged(PlayerMovement.PlayerForm newForm) 
    {
        UpdateGateCollider(newForm);
    }

    private void UpdateGateCollider(PlayerMovement.PlayerForm form)
    {
        if (solidCollider == null) return;  
        // Human → disable wall (can walk through).
        // Cat   → enable wall (physically blocked).
        solidCollider.enabled = (form == PlayerMovement.PlayerForm.Cat);
    }

    private void ShowNotification()  //display the prompt text
    {
        if (notificationText)  notificationText.text = Message;
        if (notificationPanel) notificationPanel.SetActive(true);

        if (_hideRoutine != null) StopCoroutine(_hideRoutine);
        _hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        if (notificationPanel) notificationPanel.SetActive(false);
    }

    private void OnDisable()
    {
        if (_playerMovement != null)
        {
            _playerMovement.OnFormChanged -= OnPlayerFormChanged;
            _playerMovement = null;
        }
        _collidersInZone = 0;
        if (solidCollider) solidCollider.enabled = true;
    }
}
