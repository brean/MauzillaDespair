using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    // Benutze zwei AudioSources, damit es beim Umschalten auf ein anderes
    // Lied keine Unterbrechung gibt.
    public AudioSource introMusic;
    public AudioSource backgroundMusic;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        changeToGamingMusic();
    }

    private void changeToGamingMusic()
    {
        if (!backgroundMusic.isPlaying && GameManager.instance.currentSceneName == "MainCity")
        {
            introMusic.Stop();
            backgroundMusic.Play();
        }
    }
}
