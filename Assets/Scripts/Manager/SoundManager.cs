using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    // Benutze zwei AudioSources, damit es beim Umschalten auf ein anderes
    // Lied keine allzugroße Unterbrechung gibt.
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

    // There is a introsong that loops and plays on Awake.
    // When starting the game, wait for that loop to end, and play the
    // gaming music afterwards (delayed). That makes a smooth transition.
    private void changeToGamingMusic()
    {
        if (hasNotAlreadySwitched() && GameManager.instance.currentSceneName == "MainCity")
        {
            introMusic.loop = false;
            backgroundMusic.PlayDelayed(introMusic.clip.length - introMusic.time);
        }
    }

    private bool hasNotAlreadySwitched()
    {
        return introMusic.loop;
    }
}
