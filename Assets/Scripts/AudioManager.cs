using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;


public class AudioManager : MonoBehaviour
{

    [SerializeField] EventReference mainTheme;

    // Start is called before the first frame update
    private void Start()
    {
        PlaymainTheme(); 
    }

    public void PlaymainTheme()
    {
        RuntimeManager.PlayOneShot(mainTheme);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
