using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using static PlayerMovement;

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
        HfootstepsInstance = RuntimeManager.CreateInstance(humanFootsteps);
        CfootstepsInstance = RuntimeManager.CreateInstance(catFootsteps);
        meowingInstance = RuntimeManager.CreateInstance(meowing);
        movement = player.GetComponent<PlayerMovement>();

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
    [SerializeField] EventReference catFootsteps;
    [SerializeField] float rate;
    [SerializeField] GameObject player;

    //Meowing

    [SerializeField] EventReference meowing;

    FMOD.Studio.EventInstance meowingInstance;

    FMOD.Studio.EventInstance HfootstepsInstance;
    FMOD.Studio.EventInstance CfootstepsInstance;

    float time;
    PlayerMovement movement;

    void Update()
    {

        //Handles Meowing SFX

        if (Input.GetKeyDown(KeyCode.M))
        {
            meowingInstance.start();
        }

        //Handles Footstep SFX
        float horizontal = Input.GetAxis("Horizontal");
        bool isMoving = horizontal != 0;
        bool isHuman = movement.currentForm == PlayerMovement.PlayerForm.Human;

        if (isMoving)
        {
            if (isHuman)
            {
                
                CfootstepsInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

                PLAYBACK_STATE state;
                HfootstepsInstance.getPlaybackState(out state);

                if (state != PLAYBACK_STATE.PLAYING)
                {
                    HfootstepsInstance.start();
                }
            }
            else // cat footsteps
            {
               
                HfootstepsInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

                PLAYBACK_STATE state;
                CfootstepsInstance.getPlaybackState(out state);

                if (state != PLAYBACK_STATE.PLAYING)
                {
                    CfootstepsInstance.start();
                }
            }
        }
        else
        {
            
            HfootstepsInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            CfootstepsInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }


    void OnDestroy()
    {
        HfootstepsInstance.release();
        CfootstepsInstance.release();
    }


    


    
}





   






