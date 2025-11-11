using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoorTrigger2D : MonoBehaviour
{
    [Header("References")]
    public DoorController2D door;
    public TMP_Text promptTextUI;
    public GameObject promptRoot;

    [Header("Prompt Settings")]
    [TextArea] public string prompt = "Press K to open the door";
    public KeyCode key = KeyCode.K;

    bool playerInRange;

    void Awake()
    {
        SetPromptVisible(false);
        if (promptTextUI) promptTextUI.text = prompt;
    }

    bool IsPlayer(Collider2D other)
    {
        // accept Player tag, or PlayerMovement on this object or its parents
        return other.CompareTag("Player")
            || other.GetComponent<PlayerMovement>()
            || other.GetComponentInParent<PlayerMovement>();
    }
    void Update()
    {
        if (!playerInRange) return;

        if (door && !door.IsOpen && Input.GetKeyDown(key))
        {
            door.TryOpen();
            SetPromptVisible(false);
            playerInRange = false;

            Collider2D col = GetComponent<Collider2D>();
            if (col) col.enabled = false;
            enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (door && door.IsOpen) return;

        if (IsPlayer(other))
        {
            playerInRange = true;
            if (promptTextUI) promptTextUI.text = prompt;
            SetPromptVisible(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (playerInRange && IsPlayer(other))
        {
            playerInRange = false;
            SetPromptVisible(false);
        }
    }

    void SetPromptVisible(bool visible)
    {
        if (promptRoot) promptRoot.SetActive(visible);
        else if (promptTextUI) promptTextUI.gameObject.SetActive(visible);
    }
}
