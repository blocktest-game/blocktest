using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

static class Globals {
    /// <summary> The global block manager instance. </summary>
    public static BlockManager blockManager;
    /// <summary> Array which stores all block instances for referencing as if they were globals. </summary>
    public static Block[] AllBlocks => blockManager.allBlocks;
    /// <summary> Tilemap for foreground objects. </summary>
    public static Tilemap foregroundTilemap;
    /// <summary> Tilemap for background (non-dense) objects. </summary>
    public static Tilemap backgroundTilemap;
    /// <summary> The maximum world size. (Width) </summary>
    public static int maxX = 510;
    /// <summary> The maximum world size. (Height) </summary>
    public static int maxY = 255;

    /// <summary>
    /// Helper method for converting from cell position to world position
    /// </summary>
    /// <param name="cellLocation">The location to convert to world coordinates</param>
    public static Vector3 CellToWorld(Vector3Int cellLocation) => foregroundTilemap.CellToWorld(cellLocation);
    /// <summary>
    /// Helper method for converting from cell position to world position
    /// </summary>
    /// <param name="cellLocation">The location to convert to world coordinates</param>
    public static Vector3 CellToWorld(Vector2 cellLocation) => foregroundTilemap.CellToWorld(new Vector3Int(Mathf.RoundToInt(cellLocation.x), Mathf.RoundToInt(cellLocation.y), 0));
    /// <summary>
    /// Helper method for converting from world position to approximate cell position
    /// </summary>
    /// <param name="cellLocation">The location to convert to cell coordinates</param>
    public static Vector3Int WorldToCell(Vector2 worldLocation) => foregroundTilemap.WorldToCell(worldLocation);

    /// <summary>
    /// Get a certain tile on either the foreground or background at a certain position
    /// </summary>
    /// <param name="position">The position of the tile that you want to get</param>
    /// <param name="foreground">Whether or not the tile is in the foreground</param>
    public static BlockTile GetTile(Vector3Int position, bool foreground) => foreground ? foregroundTilemap.GetTile<BlockTile>(position) : backgroundTilemap.GetTile<BlockTile>(position);
    /// <summary>
    /// Get a certain tile on a tilemap at a certain position. Used in place of Tilemap.GetTile() as that does not return a BlockTile.
    /// </summary>
    /// <param name="position">The position of the tile that you want to get</param>
    /// <param name="tilemap"></param>
    public static BlockTile GetTile(Vector3Int position, Tilemap tilemap) => tilemap.GetTile<BlockTile>(position);
    /// <summary>
    /// Get a certain tile on a tilemap at a certain position. Used in place of Tilemap.GetTile() as that does not return a BlockTile.
    /// </summary>
    /// <param name="position">The position of the tile that you want to get</param>
    /// <param name="tilemap"></param>
    public static BlockTile GetTile(Vector3Int position, ITilemap tilemap) => tilemap.GetTile<BlockTile>(position);

}
