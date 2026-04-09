using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;


    [Header("Audio Events")]
    [SerializeField] private EventReference mainTheme;

    private EventInstance currentMusic;


    private void Awake()
    {
        // Singleton (keeps one AudioManager across scenes)
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        PlayMusicForScene(SceneManager.GetActiveScene().name);
        footstepsInstance = RuntimeManager.CreateInstance(humanFootsteps);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        // Stop previous music (fade out)
        if (currentMusic.isValid())
        {
            currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusic.release();
        }

        // Choose music based on scene name
        EventReference selectedMusic = default;

        if (sceneName == "FaithAudioScene")
        {
            selectedMusic = mainTheme;
        }

        else
        {
            Debug.LogWarning("No music assigned for scene: " + sceneName);
            return;
        }

        // Play new music
        currentMusic = RuntimeManager.CreateInstance(selectedMusic);
        currentMusic.start();
    }

    public void StopMusic()
    {
        if (currentMusic.isValid())
        {
            currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusic.release();
        }
    }


    //Human Footsteps

    [SerializeField] EventReference humanFootsteps;
    [SerializeField] float rate;
    [SerializeField] GameObject player;

    FMOD.Studio.EventInstance footstepsInstance;

    float time;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        bool isMoving = horizontal != 0;

        if (isMoving)
        {
            PLAYBACK_STATE state;
            footstepsInstance.getPlaybackState(out state);

            if (state != PLAYBACK_STATE.PLAYING)
            {
                footstepsInstance.start();
            }
        }
        else
        {
            footstepsInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    void OnDestroy()
    {
        footstepsInstance.release();
    }
}






