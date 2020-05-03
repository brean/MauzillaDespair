using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Player> players = new List<Player>();
    public string currentSceneName;
    public InputType inputType;

    KeyCode nextLevelButton = KeyCode.Return;

    public GameObject mauzilla;
    public GameObject schneider;
    public GameObject maurer;
    public GameObject tischler;
    public GameObject building;
    [HideInInspector]
    public GameObject buildingsContainer;

    void Awake()
    {
        // If instance doesn't exist, set to this 
        if (instance == null)
        {
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
        createbuildings();

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

    void createPlayers()
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

    private void createbuildings()
    {
        // return;
        buildingsContainer = new GameObject();
        buildingsContainer.name = "Buildingssss";

        List<Vector3> buildingPositions = new List<Vector3>();
        buildingPositions.Add(new Vector3(1.01f, -1.1f, 0));
        buildingPositions.Add(new Vector3(-1.52f, -3.45f, 0));
        buildingPositions.Add(new Vector3(-4.99f, -2.72f, 0));
        buildingPositions.Add(new Vector3(3.51f, -3.53f, 0));
        buildingPositions.Add(new Vector3(5.46f, -1.97f, 0));

        buildingPositions.Add(new Vector3(4.493f, -1.512f, 0));
        buildingPositions.Add(new Vector3(3.46f, -1.02f, 0));
        buildingPositions.Add(new Vector3(3.02f, -0.28f, 0));
        buildingPositions.Add(new Vector3(1.46f, -0.06f, 0));
        buildingPositions.Add(new Vector3(-1f, -0.31f, 0));

        buildingPositions.Add(new Vector3(-3.02f, -0.23f, 0));
        buildingPositions.Add(new Vector3(-5.5f, 0.01f, 0));
        buildingPositions.Add(new Vector3(-5.97f, 2.28f, 0));
        buildingPositions.Add(new Vector3(-2.97f, 2.8f, 0));
        buildingPositions.Add(new Vector3(0.98f, 3.76f, 0));

        buildingPositions.Add(new Vector3(0.98f, 2.67f, 0));
        buildingPositions.Add(new Vector3(-0.49f, 2f, 0));
        buildingPositions.Add(new Vector3(0.98f, 1.78f, 0));
        buildingPositions.Add(new Vector3(3.01f, 2.3f, 0));
        buildingPositions.Add(new Vector3(3.93f, 2.76f, 0));

        buildingPositions.Add(new Vector3(6.01f, 3.34f, 0));
        buildingPositions.Add(new Vector3(5.03f, 0.85f, 0));

        List<Vector3Int> buildingsTileMapPositions = new List<Vector3Int>();
        buildingsTileMapPositions.Add(new Vector3Int(-2, -4, 0));
        buildingsTileMapPositions.Add(new Vector3Int(-9, -6, 0));
        buildingsTileMapPositions.Add(new Vector3Int(-11, -1, 0));
        buildingsTileMapPositions.Add(new Vector3Int(-4, -11, 0));
        buildingsTileMapPositions.Add(new Vector3Int(1, -10, 0));

        buildingsTileMapPositions.Add(new Vector3Int(1, -8, 0));
        buildingsTileMapPositions.Add(new Vector3Int(1, -6, 0));
        buildingsTileMapPositions.Add(new Vector3Int(2, -4, 0));
        buildingsTileMapPositions.Add(new Vector3Int(1, -2, 0));
        buildingsTileMapPositions.Add(new Vector3Int(-2, 0, 0));

        buildingsTileMapPositions.Add(new Vector3Int(-4, 2, 0));
        buildingsTileMapPositions.Add(new Vector3Int(-6, 5, 0));
        buildingsTileMapPositions.Add(new Vector3Int(-2, 10, 0));
        buildingsTileMapPositions.Add(new Vector3Int(2, 8, 0));
        buildingsTileMapPositions.Add(new Vector3Int(8, 6, 0));

        buildingsTileMapPositions.Add(new Vector3Int(6, 4, 0));
        buildingsTileMapPositions.Add(new Vector3Int(3, 4, 0));
        buildingsTileMapPositions.Add(new Vector3Int(4, 2, 0));
        buildingsTileMapPositions.Add(new Vector3Int(7, 1, 0));
        buildingsTileMapPositions.Add(new Vector3Int(9, 1, 0));

        buildingsTileMapPositions.Add(new Vector3Int(12, 0, 0));
        buildingsTileMapPositions.Add(new Vector3Int(6, -4, 0));


        int index = 0;
        foreach (Vector3 position in buildingPositions)
        {
            GameObject instantiatedBuilding = Instantiate(building, position, Quaternion.identity);
            instantiatedBuilding.GetComponent<Building>().positionInTileMap = buildingsTileMapPositions[index];
            instantiatedBuilding.name = "Building " + index;
            instantiatedBuilding.transform.parent = buildingsContainer.transform;   
            index++;
        }
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
            if (GameObject.Find("Mauzilla").GetComponent<MauzillaPlayer>().health <= 10) {
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
