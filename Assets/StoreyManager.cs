using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// manages the level of the building
public class StoreyManager : MonoBehaviour
{

    public Vector3Int positionInTileMap;
    public BuildingManager buildingManager;
    // Start is called before the first frame update

    public void DestroyBuilding() {
        buildingManager.DestroyBuilding(positionInTileMap);
    }

    public void RepairBuilding() {
        buildingManager.RepairBuilding(positionInTileMap);
    }

    void Start()
    {
        buildingManager = GameObject.Find("Managers").GetComponent<BuildingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
