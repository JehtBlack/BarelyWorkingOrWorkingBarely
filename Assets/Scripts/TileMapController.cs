using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileMapController : MonoBehaviour {

    public List<TileBase> PrimaryTileSet;
    public List<TileBase> SecondaryTileSet;

    private Tilemap Map;
    private bool PrimaryIsActive = true;

    void Awake() {
        Map = GetComponent<Tilemap>();
    }

    public void SwapTileset() {
        int tileCount = Mathf.Min(PrimaryTileSet.Count, SecondaryTileSet.Count);
        List<TileBase> from;
        List<TileBase> to;
        if (PrimaryIsActive) {
            from = PrimaryTileSet;
            to = SecondaryTileSet;
        }
        else {
            from = SecondaryTileSet;
            to = PrimaryTileSet;
        }

        for (int i = 0; i < tileCount; ++i) {
            Map.SwapTile(from[i], to[i]);
        }

        PrimaryIsActive = !PrimaryIsActive;
    }
}
