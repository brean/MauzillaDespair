using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    // public List<PlayerNew> playersNew = new List<PlayerNew>();

    public List<Player> players = new List<Player>();
    public string currentSceneName;
    public InputType inputType;

    KeyCode nextLevelButton = KeyCode.Return;

    public GameObject mauzilla;
    public GameObject schneider;
    public GameObject maurer;
    public GameObject tischler;

    void Awake()
    {
        // If instance doesn't exist, set to this 
        if (instance == null) {
            // Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        // If instance already exists and it's not this, destroy this (enforces
        // our singleton pattern, meaning there can only ever be one instance of a GameManager)
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Sets initially currentScenename. Should be "Start"
        currentSceneName = SceneManager.GetActiveScene().name;

        createPlayers();

        // activeSceneChanged is a Event that is fired, when SceneManager.LoadScene()
        // is called. Also fired multiple times on init.
        // The operation += adds a method that will be called when the event happens
        SceneManager.activeSceneChanged += changeCurrentSceneName;
    }

    void changeCurrentSceneName(Scene previousScene, Scene newScene)
    {
        // previousScene seems to be null all the time
        currentSceneName = newScene.name;
    }

    void Update()
    {
        checkSceneChange();
    }

    public void createPlayers()
    {
        if (currentSceneName == "MainCity") {
            mauzilla = Instantiate(mauzilla, new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
            mauzilla.name = "Mauzilla";
            schneider = Instantiate(schneider, new Vector3(0.5f, -0.5f, 0), Quaternion.identity);
            schneider.name = "Schneider";
            maurer = Instantiate(maurer, new Vector3(-0.5f, 0.5f, 0), Quaternion.identity);
            maurer.name = "Maurer";
            tischler = Instantiate(tischler, new Vector3(-0.5f, -0.5f, 0), Quaternion.identity);
            tischler.name = "Tischler";
        }

        players = new List<Player>
        {
            new Player{
                character = Character.mauzilla,
                active = true,
                ready = true
            },
            new Player{
                character = Character.schneider,
                active = true,
                ready = true
            },
            new Player{
                character = Character.maurer,
                active = true,
                ready = true
            },
            new Player{
                character = Character.tischler,
                active = true,
                ready = true
            }
        };
    }

    void checkSceneChange()
    {
        if (currentSceneName == "Start" && Input.GetKeyDown(nextLevelButton))
        {
            SceneManager.LoadScene("MainCity");
        }
        if (currentSceneName == "MainCity")
        {
            if (GameObject.Find("CityHealthbar").GetComponent<CityHealthbar>().health <= 10) {
                SceneManager.LoadScene("MauzillaWins");
            }
            if (GameObject.Find("Mauzilla").GetComponent<Mauzilla>().health <= 10) {
                SceneManager.LoadScene("ArtisansWin");
            }
        }
        if (Input.GetKeyDown(nextLevelButton)
            && (currentSceneName == "ArtisansWins" || currentSceneName == "MauzillaWins")
        ) {
            SceneManager.LoadScene("Start");
        }
    }

    public Player playerForCharacter(Character character)
    {
        foreach (Player p in players)
        {
            if (p.character == character)
            {
                return p;
            }
        }

        return null;
    }
}
