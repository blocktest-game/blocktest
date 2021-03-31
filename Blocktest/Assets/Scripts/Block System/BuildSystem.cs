using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class BuildSystem
{
    /// <summary>
    /// An array containing an entry for every block in the world. Used for saving games.
    /// </summary>
    public static int[,,] currentWorld = new int[Globals.maxX, Globals.maxY, 2];

    /// <summary>
    /// The method called whenever an object is removed.
    /// </summary>
    /// <param name="foreground">Whether or not the block to be destroyed is in the foreground.</param>
    /// <param name="position">The position of the block to destroy (world coords)</param>
    public static void BreakBlockWorld(bool foreground, Vector2 position) => BreakBlockCell(foreground, Globals.foregroundTilemap.WorldToCell(position));

    /// <summary>
    /// The method called whenever an object is removed.
    /// </summary>
    /// <param name="foreground">Whether or not the block to be destroyed is in the foreground.</param>
    /// <param name="tilePosition">The position of the block to destroy (grid coords)</param>
    public static void BreakBlockCell(bool foreground, Vector3Int tilePosition)
    {
        if (foreground && Globals.foregroundTilemap.HasTile(tilePosition)) {
            BlockTile prevTile = Globals.GetTile(tilePosition, foreground);
            prevTile.sourceBlock.OnBreak(tilePosition, true);

            Globals.foregroundTilemap.SetTile(tilePosition, null);
            currentWorld[tilePosition.x, tilePosition.y, 0] = 0;
        } else if (!foreground && Globals.backgroundTilemap.HasTile(tilePosition)) {
            BlockTile prevTile = Globals.GetTile(tilePosition, foreground);
            prevTile.sourceBlock.OnBreak(tilePosition, false);

            Globals.backgroundTilemap.SetTile(tilePosition, null);
            currentWorld[tilePosition.x, tilePosition.y, 1] = 0;
        }

        Tilemap tilemap = foreground ? Globals.foregroundTilemap : Globals.backgroundTilemap;

        foreach (Vector3Int loc in new List<Vector3Int>() { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right }) { // Refreshes all blocks in cardinal dirs
            tilemap.RefreshTile(tilePosition + loc);
        }

    }

    /// <summary>
    /// The method called whenever a block is placed.
    /// </summary>
    /// <param name="toPlace">The block type to place.</param>
    /// <param name="foreground">Whether or not the block should be placed in the foreground.</param>
    /// <param name="position">The position of the placed block. (World coords)</param>
    public static void PlaceBlockWorld(Block toPlace, bool foreground, Vector2 position) => PlaceBlockCell(toPlace, foreground, Globals.foregroundTilemap.WorldToCell(position));

    /// <summary>
    /// The method called whenever a block is placed.
    /// </summary>
    /// <param name="toPlace">The block type to place.</param>
    /// <param name="foreground">Whether or not the block should be placed in the foreground.</param>
    /// <param name="position">The position of the placed block. (Grid coords)</param>
    public static void PlaceBlockCell(Block toPlace, bool foreground, Vector3Int tilePosition)
    {
        BlockTile newTile = BlockTile.CreateInstance<BlockTile>();
        newTile.sourceBlock = toPlace;
        newTile.sprite = toPlace.blockSprite;
        newTile.name = toPlace.blockName;
        toPlace.OnPlace(tilePosition, foreground);

        if (foreground) {
            newTile.colliderType = Tile.ColliderType.Grid;
            Globals.foregroundTilemap.SetTile(tilePosition, newTile);
            currentWorld[tilePosition.x, tilePosition.y, 0] = toPlace.blockID + 1;
        } else if (toPlace.canPlaceBackground) {
            newTile.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            Globals.backgroundTilemap.SetTile(tilePosition, newTile);
            currentWorld[tilePosition.x, tilePosition.y, 1] = toPlace.blockID + 1;
        }
    }

    /// <summary>
    /// The method called whenever a block is placed.
    /// </summary>
    /// <param name="toPlace">The block type to place.</param>
    /// <param name="foreground">Whether or not the block should be placed in the foreground.</param>
    /// <param name="tilePosition">The position of the placed block. (Grid coords)</param>
    public static void PlaceBlockCell(Block toPlace, bool foreground, Vector2 tilePosition) => PlaceBlockCell(toPlace, foreground, new Vector3Int(Mathf.RoundToInt(tilePosition.x), Mathf.RoundToInt(tilePosition.y), 0));

    public static void PlaceIDsCells(int[] blockIDList, bool foreground, Vector3Int[] tilePositions)
    {
        BlockTile[] tilesToPlace = new BlockTile[blockIDList.Length];
        for(int i = 0; i < blockIDList.Length; i++) {
            if(blockIDList[i] == 0) { continue; }
            BlockTile newTile = BlockTile.CreateInstance<BlockTile>();
            Vector3Int tilePosition = tilePositions[i];
            Block toPlace = Globals.AllBlocks[blockIDList[i] - 1];
            newTile.sourceBlock = toPlace;
            newTile.sprite = toPlace.blockSprite;
            newTile.name = toPlace.blockName;

            if(foreground) {
                newTile.colliderType = Tile.ColliderType.Grid;
            } else {
                newTile.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
            tilesToPlace[i] = newTile;
        }
        if(foreground) {
            Globals.foregroundTilemap.SetTiles(tilePositions, tilesToPlace);
        } else {
            Globals.backgroundTilemap.SetTiles(tilePositions, tilesToPlace);
        }
    }
}

public class BlockTile : Tile
{
    /// <summary>
    /// The type of block this tile is.
    /// </summary>
    public Block sourceBlock;

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
        foreach (Vector3Int dir in new List<Vector3Int>() { Vector3Int.up, Vector3Int.down, Vector3Int.right, Vector3Int.left }) {
            if (HasSmoothableTile(position + dir, tilemap)) {
                tilemap.RefreshTile(position + dir); // This doesn't actuall call this same method, but the followind GetTileData() method, so don't worry about infinite loops.
            }
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        if (!sourceBlock.blockSmoothing || (sourceBlock.spriteSheet == null)) {
            base.GetTileData(position, tilemap, ref tileData);
            return;
        } // If the tile doesn't or can't smooth, don't even try

        int bitmask = 0; // Using bitmask smoothing, look it up

        if (HasSmoothableTile(position + Vector3Int.up, tilemap)) {
            bitmask += 1;
        }
        if (HasSmoothableTile(position + Vector3Int.down, tilemap)) {
            bitmask += 2;
        }
        if (HasSmoothableTile(position + Vector3Int.right, tilemap)) {
            bitmask += 4;
        }
        if (HasSmoothableTile(position + Vector3Int.left, tilemap)) {
            bitmask += 8;
        }

        sprite = sourceBlock.spriteSheet.spritesDict[sourceBlock.blockSprite.texture.name + "_" + bitmask];
        base.GetTileData(position, tilemap, ref tileData);
    }

    /// <summary>
    /// Whether or not the tile at a certain <paramref name="position"/> can smooth with this tile.
    /// </summary>
    /// <param name="position">The position of the tile to check for smoothing.</param>
    /// <param name="tilemap">The tilemap on which the tile you want to check for smoothing is.</param>
    /// <returns>Whether or not the tile can smooth with this tile.</returns>
    private bool HasSmoothableTile(Vector3Int position, ITilemap tilemap)
    {
        if (sourceBlock.smoothSelf) { return IsSameTileType(Globals.GetTile(position, tilemap)); }
        return Globals.GetTile(position, tilemap) != null;
    }

    /// <summary>
    /// If the tile provided is the same type (references the same block) as the current tile.
    /// </summary>
    /// <param name="otherBlock">The other tile to check.</param>
    /// <returns>Whether or not the other block is the same type as the current tile</returns>
    private bool IsSameTileType(BlockTile otherTile) => otherTile?.sourceBlock == sourceBlock;

}
