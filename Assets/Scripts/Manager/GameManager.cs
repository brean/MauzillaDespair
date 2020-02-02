using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public List<Player> players = new List<Player>();
    public int winningTeam;

    public string previousSceneName;
    public string currentSceneName;

    // Start is called before the first frame update
    void Awake()
    {
        //If instance doesn't exist, set to this 
        if (instance == null) { instance = this; }
        //If instance already exists and it's not this, destroy this (enforces our singleton pattern, meaning there can only ever be one instance of a GameManager)
        else if (instance != this) { Destroy(gameObject); }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Manage Scenes
        currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.activeSceneChanged += gettingSceneInfo;
    }

    void gettingSceneInfo(Scene previousScene, Scene newScene) {
        previousSceneName = currentSceneName;
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    void Update() {
        if (currentSceneName == "Start" && Input.GetKeyDown("space")) SceneManager.LoadScene("MainCity");
        if (currentSceneName == "MainCity") {
            if (GameObject.Find("CityHealthbar").GetComponent<CityHealthbar>().health <= 10) SceneManager.LoadScene("MauzillaWins");
            if (GameObject.Find("Mauzilla").GetComponent<Mauzilla>().health <= 10) SceneManager.LoadScene("ArtisansWins");
        }
        if (currentSceneName == "ArtisansWins" || currentSceneName == "MauzillaWins" && Input.GetKeyDown("space")) SceneManager.LoadScene("Start");
    }

    public static Player getPlayerForCharacter(Character character) {
        if (instance == null) {
            instance = new GameManager();
        }
        return instance.playerForCharacter(character);
    }

    public Player playerForCharacter(Character character)
    {
        if (players.Count != 4)
        {
            // quickly fake inputs for testing
            players = new List<Player>
            {
                new Player{
                    character = Character.mauzilla,
                    inputType = InputType.All,
                    number = 1,
                    active = true,
                    ready = true
                },
                new Player{
                    character = Character.schneider,
                    inputType = InputType.All,
                    number = 2,
                    active = true,
                    ready = true
                },
                new Player{
                    character = Character.maurer,
                    inputType = InputType.All,
                    number = 3,
                    active = true,
                    ready = true
                },
                new Player{
                    character = Character.tischler,
                    inputType = InputType.All,
                    number = 4,
                    active = true,
                    ready = true
                }
            };
        }
        foreach (Player p in players) {
            if (p.character == character) {
                return p;
            }
        }
        return null;
    }

    public void loadScene(string scene) {
        SceneManager.LoadScene(scene);
    }
}
