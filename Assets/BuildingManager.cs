using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class BuildingManager : MonoBehaviour
{
    public List<Tilemap> tileMaps;
    public List<Tilemap> ignoreMaps;
    void OnValidate() {
        Awake();
    }

    // Start is called before the first frame update
    void Awake()
    {
        tileMaps = new List<Tilemap>();
        foreach(Transform child in transform)
        {
            tileMaps.Add(child.gameObject.GetComponent<Tilemap>());
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
