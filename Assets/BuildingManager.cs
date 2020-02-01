using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingManager : MonoBehaviour
{
    public TileBase damageTile;
    public TileBase origTile;
    public Tilemap groundLevelMap;
    public Vector3Int tilePosition;
    // Start is called before the first frame update
    void Start()
    {
        // origTile = groundLevelMap.GetTile(tilePosition);
        //groundLevelMap.SetTile(tilePosition, damageTile);
        // groundLevelMap.getTile()
        // groundLevelMap.setTile()
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
