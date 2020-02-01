using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// manages the level of the building
public class StoreyManager : MonoBehaviour
{

    public Vector3Int positionInTileMap;
    public List<TileBase> storeysAfterRepair;
    public TileBase destroyed;
    public BuildingManager buildingManager;
    // Start is called before the first frame update

    public void DestroyBuilding() {
        buildingManager.DestroyBuilding(positionInTileMap);
        buildingManager.tileMaps[1].SetTile(positionInTileMap, destroyed);
    }

    public void RepairBuilding() {
        buildingManager.SetBuilding(positionInTileMap, storeysAfterRepair);
    }

    void Start()
    {
        buildingManager = GameObject.Find("Grid - Level").GetComponent<BuildingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
