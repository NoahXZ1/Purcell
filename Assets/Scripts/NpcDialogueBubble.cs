using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NpcDialogueBubble : MonoBehaviour
{
    [Header("UI")]
    public GameObject bubbleRoot;    //the bubble panel object.
    public TMP_Text bubbleText;      //text component inside the bubble.

    [Header("Dialogue")]
    [TextArea]
    public string[] lines;           //all dialogue lines.
    public KeyCode interactKey = KeyCode.P;

    [Header("Quest")]
    [TextArea]
    public string nextQuestText;     //mission tips text to set after all lines are finished

    private bool playerInRange;
    private bool dialogueActive;
    private int currentIndex = -1;

    private bool questUpdated;       //to make sure we only update quest once
    private void Start()
    {
        if (bubbleRoot != null)
        {
            bubbleRoot.SetActive(false);
        }
    }

    private void Update()
    {
        //bubble won't show up when player not near npc. 
        if (!playerInRange)
        {
            return;
        }

        //Player presses "P" to go to next line or close.
        if (Input.GetKeyDown(interactKey))
        {
            if (!dialogueActive)
            {
                StartDialogue();
            }
            else
            {
                NextLine();
            }
        }
    }

    private void StartDialogue()
    {
        if (lines == null || lines.Length == 0 || bubbleRoot == null || bubbleText == null)
            return;

        dialogueActive = true;
        currentIndex = 0;

        bubbleRoot.SetActive(true);
        bubbleText.text = lines[currentIndex];
    }

    private void NextLine()
    {
        currentIndex++;

        if (currentIndex < lines.Length)
        {
            bubbleText.text = lines[currentIndex];
        }
        else
        {
            EndDialogue(true);
        }
    }

    private void EndDialogue(bool completedAllLines = false)
    {
        //disable the dialog when the conversation ends. 
        dialogueActive = false;
        currentIndex = -1;

        if (bubbleRoot != null)
        {
            bubbleRoot.SetActive(false);
        }

        //only update quest if player really finished all lines (not just walked away)
        if (completedAllLines && !questUpdated && QuestUIManager.Instance != null)
        {
            if (!string.IsNullOrEmpty(nextQuestText))
            {
                QuestUIManager.Instance.SetQuest(nextQuestText);
                questUpdated = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = true;

        // Auto popup when player gets close.
        StartDialogue();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        //Disable dialog bubble when player walk away. 
        playerInRange = false;
        EndDialogue(false);
    }
}
