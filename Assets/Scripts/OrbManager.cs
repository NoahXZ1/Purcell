using System;
using UnityEngine;

// Tracks orb collection progress for the Cat -> Human form transition.
// Attach to the Player GameObject alongside PlayerMovement.
public class OrbManager : MonoBehaviour
{
    public const int OrbsRequired = 3;

    public int OrbCount { get; private set; } = 0;

    // Fired whenever the orb count changes; passes the new count.
    public event Action<int> OnOrbCountChanged;

    public bool CanTransformToHuman => OrbCount >= OrbsRequired;

    // Called by OrbCollectible when the player (in Cat form) touches an orb.
    public void CollectOrb()
    {
        if (OrbCount >= OrbsRequired) return;
        OrbCount++;
        OnOrbCountChanged?.Invoke(OrbCount);
    }

    // Called by PlayerMovement when switching to Cat form — resets progress and respawns orbs.
    public void ResetOrbs()
    {
        OrbCount = 0;
        OnOrbCountChanged?.Invoke(OrbCount);
        OrbRespawnManager.Instance?.RespawnAll();
    }
}
