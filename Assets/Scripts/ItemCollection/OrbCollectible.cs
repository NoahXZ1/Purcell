using UnityEngine;

// Attach this to an Orb GameObject (requires a trigger Collider2D).
// The orb can only be collected when the player is in Cat form.
// Replaces AutoCollectItem on orb prefabs.
public class OrbCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var movement = other.GetComponent<PlayerMovement>()
                    ?? other.GetComponentInParent<PlayerMovement>();
        if (movement == null) return;

        // Only collectable in Cat form
        if (movement.currentForm != PlayerMovement.PlayerForm.Cat) return;

        var orbManager = movement.GetComponent<OrbManager>();
        if (orbManager == null) return;

        // Already full — do nothing (don't destroy the orb either)
        if (orbManager.CanTransformToHuman) return;

        orbManager.CollectOrb();
        Destroy(gameObject);
    }
}
