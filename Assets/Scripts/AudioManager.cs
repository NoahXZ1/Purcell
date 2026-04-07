using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    PlayerMovement movement;

    [Header("Music Events")]
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

        movement = GetComponent<PlayerMovement>();

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

    private void Start()
    {
        PlayMusicForScene(SceneManager.GetActiveScene().name);
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

    public void PlayWalkingMusic(){
        if(movement.inputX != 0f && movement.isGrounded){
            //Play walking music
        }
    }

    public void StopMusic()
    {
        if (currentMusic.isValid())
        {
            currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusic.release();
        }

    }

    public void StopPlayingWalkingMusic(){
        if(movement.inputX == 0f || movement.isGrounded != true){
            //Stop playing walking music
        }
    }


    //Human Footsteps

    [SerializeField] EventReference humanFootsteps;
    [SerializeField] float rate;
    [SerializeField] GameObject player;


    float time;

    public void PlayHumanFootsteps()
    {
        RuntimeManager.PlayOneShotAttached(humanFootsteps, player);
    }

    private void Update()
    {
        time += Time.deltaTime;

        //playing walking music
        PlayWalkingMusic();
        StopPlayingWalkingMusic();
    }


}

