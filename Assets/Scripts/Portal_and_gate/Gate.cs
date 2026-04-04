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
            if (_playerMovement != null)
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

        UpdateGateCollider(movement);

        // Human is now overlapping the gate — lock form changing so the player
        // cannot switch to Cat (which would get stuck inside the solid collider).
        if (movement.currentForm == PlayerMovement.PlayerForm.Human)
        {
            movement.canChangeForm = false;
        }
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

            // Player has fully left — restore form switching and the solid wall.
            movement.canChangeForm = true;
            _playerMovement.OnFormChanged -= OnPlayerFormChanged;
            _playerMovement = null;

            if (solidCollider) solidCollider.enabled = true;
        }
    }

    // Listener for OnFormChanged — signature must match Action<PlayerForm>.
    private void OnPlayerFormChanged(PlayerMovement.PlayerForm form)
    {
        UpdateGateCollider(_playerMovement);
    }

    // Controls only the solid collider; canChangeForm is managed by Enter/Exit.
    private void UpdateGateCollider(PlayerMovement movement)
    {
        if (solidCollider == null || movement == null) return;
        // Human → disable wall (can walk through).
        // Cat   → enable wall (physically blocked).
        solidCollider.enabled = (movement.currentForm == PlayerMovement.PlayerForm.Cat);
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
            _playerMovement.canChangeForm = true;  // always restore on gate disable
            _playerMovement.OnFormChanged -= OnPlayerFormChanged;
            _playerMovement = null;
        }
        _collidersInZone = 0;
        if (solidCollider) solidCollider.enabled = true;
    }
}
