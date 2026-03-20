using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private EventReference mainTheme;

    private EventInstance mainThemeInstance;

    private void Start()
    {
        PlayMainTheme();
    }

    public void PlayMainTheme()
    {
        mainThemeInstance = RuntimeManager.CreateInstance(mainTheme);
        mainThemeInstance.start();
    }

    private void OnDestroy()
    {
        StopMainTheme();
    }

    public void StopMainTheme()
    {
        if (mainThemeInstance.isValid())
        {
            mainThemeInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            mainThemeInstance.release();
        }
    }
}
