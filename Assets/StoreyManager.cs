using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// manages the level of the building
[ExecuteInEditMode]
public class StoreyManager : MonoBehaviour
{

    public Vector3Int positionInTileMap;
    public List<TileBase> storeysAtBeginning;
    public List<TileBase> storeysAfterRepair;
    public TileBase destroyed;
    public BuildingManager buildingManager;
    // Start is called before the first frame update

    void OnValidate()
    {
        if (positionInTileMap != null && buildingManager != null) {
            storeysAtBeginning = buildingManager.GetTilesAt(positionInTileMap);
        }
        
    }

    void Start()
    {
        storeysAtBeginning = buildingManager.GetTilesAt(positionInTileMap);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
