using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

static class Globals {
    public static BlockManager blockManager;
    public static Block[] allBlocks => blockManager.allBlocks;
    public static Tilemap foregroundTilemap;
    public static Tilemap backgroundTilemap;
    // World dimensions
    public static int maxX = 510;
    public static int maxY = 255;

    // Helper methods for converting from cell position to world position and vice-versa
    public static Vector3 CellToWorld(Vector3Int cellLocation) => foregroundTilemap.CellToWorld(cellLocation);
    public static Vector3 CellToWorld(Vector2 cellLocation) => foregroundTilemap.CellToWorld(new Vector3Int(Mathf.RoundToInt(cellLocation.x), Mathf.RoundToInt(cellLocation.y), 0));
    public static Vector3Int WorldToCell(Vector2 worldLocation) => foregroundTilemap.WorldToCell(worldLocation);

    // Get a certain tile on either the foreground or background at a certain position
    public static TileBase GetTile(Vector3Int position, bool foreground) => foreground ? foregroundTilemap.GetTile(position) : backgroundTilemap.GetTile(position);
}
