using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class BuildingManager : MonoBehaviour
{
    public TileBase destroyed;
    public List<TileBase> fixedGroundFloors;
    public List<TileBase> fixedStoreys;
    public List<TileBase> fixedRoof;

    public List<Tilemap> tileMaps;
    public List<Tilemap> ignoreMaps;
    public Tilemap buildingsLevel0;
    public List<Tilemap> storeysLevel;


    void OnValidate() {
        Awake();
    }

    // Start is called before the first frame update
    void Awake()
    {
        storeysLevel = new List<Tilemap>();
        tileMaps = new List<Tilemap>();
        foreach(Transform child in transform)
        {
            Tilemap tm = child.gameObject.GetComponent<Tilemap>();
            if (tm == null) {
                continue;
            }
            if (child.name.Contains("Level") && child.name.Contains("0")) {
                buildingsLevel0 = tm;
            } 
            if (child.name.Contains("Level") && !child.name.Contains("0")  && !child.name.Contains("Ground")) {
                storeysLevel.Add(tm);
            } 
            tileMaps.Add(tm);
        }
    }

    public List<TileBase> GetTilesAt(Vector3Int tilePosition) 
    {
        List<TileBase> tiles = new List<TileBase>();
        foreach (Tilemap tm in tileMaps) 
        {
            if (ignoreMaps.Contains(tm)) {
                continue;
            }
            tiles.Add(tm.GetTile(tilePosition));
        }
        return tiles;
    }

    public void DestroyBuilding(Vector3Int tilePosition) {
        foreach (Tilemap tm in tileMaps) 
        {
            if (ignoreMaps.Contains(tm)) {
                continue;
            }
            tm.SetTile(tilePosition, null);
        }
        buildingsLevel0.SetTile(tilePosition, destroyed);
    }

    public void RepairBuilding(Vector3Int tilePosition) {
        TileBase tile;
        tile = fixedGroundFloors[Random.Range(0, fixedGroundFloors.Count)];
        buildingsLevel0.SetTile(tilePosition, tile);
        int currentLevel = 0;
        for (int i = 0; i < Random.Range(0, 4); i++)
        {
            tile = fixedStoreys[Random.Range(0, fixedStoreys.Count)];
            storeysLevel[currentLevel].SetTile(tilePosition, tile);
            currentLevel++;
        }
        tile = fixedRoof[Random.Range(0, fixedRoof.Count)];
        storeysLevel[currentLevel].SetTile(tilePosition, tile);

    }

    public void SetBuilding(Vector3Int tilePosition, List<TileBase> storeys) {
        int i = 0; // counter for storey tiles
        foreach (Tilemap tm in tileMaps) {
            if (ignoreMaps.Contains(tm)) {
                continue;
            }
            tm.SetTile(tilePosition, storeys[i++]);
            if (i == storeys.Count) {
                return;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
