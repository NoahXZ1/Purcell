using System.Collections.Generic;
using UnityEngine;

// Scene-level singleton. OrbCollectible instances register themselves here on Start.
// Call RespawnAll() to re-activate every orb at its original position.
public class OrbRespawnManager : MonoBehaviour
{
    public static OrbRespawnManager Instance { get; private set; }

    private readonly List<OrbCollectible> _orbs = new List<OrbCollectible>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Register(OrbCollectible orb)
    {
        if (!_orbs.Contains(orb))
            _orbs.Add(orb);
    }

    public void RespawnAll()
    {
        foreach (var orb in _orbs)
        {
            if (orb != null)
                orb.gameObject.SetActive(true);
        }
    }
}
