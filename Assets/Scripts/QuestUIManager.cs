using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUIManager : MonoBehaviour
{
    public static QuestUIManager Instance;

    [SerializeField] private TextMeshProUGUI questText;

    private void Awake()
    {
        // Singleton pattern (simple version)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        // Optional: set an initial quest text
        SetQuest("Tip: Try find Hana!");
    }

    public void SetQuest(string newText)
    {
        if (questText == null)
        {
            Debug.LogWarning("QuestUIManager: questText reference is missing.");
            return;
        }

        questText.text = newText;
    }
}
