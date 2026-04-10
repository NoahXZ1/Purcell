using UnityEngine;
using UnityEngine.UI;

// Controls the orb progress bar shown above the player's head in Cat form.
//
// Setup in Unity:
//   1. Create a child Canvas on the Player (Render Mode: World Space).
//      Set its width/height and scale so it looks right above the player sprite.
//   2. Inside the Canvas add 3 Image objects (one per orb slot) arranged horizontally.
//   3. Attach this script to the Canvas (or any GameObject inside it).
//   4. Assign the 3 Image references to orbSegments[].
//      The Canvas root itself acts as uiRoot (or assign a different object).
//
// The bar is visible only in Cat form and fills one segment per collected orb.
public class OrbProgressUI : MonoBehaviour
{
    [Header("Segment Images (one per orb slot, in order)")]
    [SerializeField] private Image[] orbSegments = new Image[3];

    [Header("Colors")]
    [SerializeField] private Color filledColor  = new Color(1.0f, 0.85f, 0.1f, 1.0f); // gold
    [SerializeField] private Color emptyColor   = new Color(1.0f, 1.0f,  1.0f, 0.25f); // faded white

    [Header("Root to show/hide (defaults to this GameObject)")]
    [SerializeField] private GameObject uiRoot;

    private OrbManager      orbManager;
    private PlayerMovement  playerMovement;

    private void Awake()
    {
        // Support both: script on a child of Player, or somewhere else in the scene.
        playerMovement = GetComponentInParent<PlayerMovement>();
        if (playerMovement == null)
        {
            var playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) playerMovement = playerObj.GetComponent<PlayerMovement>();
        }

        if (playerMovement == null)
        {
            Debug.LogWarning("OrbProgressUI: could not find PlayerMovement.");
            return;
        }

        orbManager = playerMovement.GetComponent<OrbManager>();
        if (orbManager == null)
            Debug.LogWarning("OrbProgressUI: OrbManager not found on Player.");

        if (uiRoot == null) uiRoot = gameObject;

        // Subscribe in Awake/OnDestroy (NOT OnEnable/OnDisable).
        // Reason: uiRoot may be this gameObject itself. If so, Start() calls
        // SetActive(false) which triggers OnDisable and silently unsubscribes
        // the events, permanently breaking the bar.
        if (orbManager     != null) orbManager.OnOrbCountChanged += RefreshSegments;
        if (playerMovement != null) playerMovement.OnFormChanged += OnFormChanged;
    }

    private void OnDestroy()
    {
        if (orbManager     != null) orbManager.OnOrbCountChanged -= RefreshSegments;
        if (playerMovement != null) playerMovement.OnFormChanged -= OnFormChanged;
    }

    private void Start()
    {
        bool isCat = playerMovement != null &&
                     playerMovement.currentForm == PlayerMovement.PlayerForm.Cat;
        uiRoot.SetActive(isCat);
        RefreshSegments(orbManager?.OrbCount ?? 0);
    }

    private void OnFormChanged(PlayerMovement.PlayerForm form)
    {
        uiRoot.SetActive(form == PlayerMovement.PlayerForm.Cat);
    }

    private void RefreshSegments(int count)
    {
        for (int i = 0; i < orbSegments.Length; i++)
        {
            if (orbSegments[i] == null) continue;
            orbSegments[i].color = (i < count) ? filledColor : emptyColor;
        }
    }
}
