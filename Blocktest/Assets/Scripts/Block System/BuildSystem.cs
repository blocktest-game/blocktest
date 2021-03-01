using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public static class BuildSystem
{

    [SerializeField] public static int[,,] currentWorld = new int[Globals.maxX, Globals.maxY, 2];

    /// <summary>
    /// The method called whenever an object is removed.
    /// </summmary>
    /// <param name="foreground"> Whether or not the block to be destroyed is in the foreground. </param>
    /// <param name="position"> The position of the block to destroy (world coords) </param>
    public static void BreakBlockWorld(bool foreground, Vector2 position) => BreakBlockCell(foreground, Globals.foregroundTilemap.WorldToCell(position));

    /// <summary>
    /// The method called whenever an object is removed.
    /// </summmary>
    /// <param name="foreground"> Whether or not the block to be destroyed is in the foreground. </param>
    /// <param name="position"> The position of the block to destroy (grid coords) </param>
    public static void BreakBlockCell(bool foreground, Vector3Int tilePosition)
    {
        if(foreground && Globals.foregroundTilemap.HasTile(tilePosition)) {
            Globals.foregroundTilemap.SetTile(tilePosition, null);
            currentWorld[tilePosition.x, tilePosition.y, 0] = 0;
        } else if (!foreground && Globals.backgroundTilemap.HasTile(tilePosition)) {
            Globals.backgroundTilemap.SetTile(tilePosition, null);
            currentWorld[tilePosition.x, tilePosition.y, 1] = 0;
        }

        Tilemap tilemap = foreground ? Globals.foregroundTilemap : Globals.backgroundTilemap;

        foreach (Vector3Int loc in new List<Vector3Int>(){Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right}) { // Refreshes all blocks in cardinal dirs
            tilemap.RefreshTile(tilePosition + loc);
        }

    }

    //
    // Summary:
    //      The method called whenever a block is placed.
    // Parameters:
    //      toPlace:
    //          The block type to place.
    //      foreground:
    //          Whether or not the block should be placed in the foreground.
    //      position:
    //          The position of the placed block
    public static void PlaceBlockWorld(Block toPlace, bool foreground, Vector2 position) => PlaceBlockCell(toPlace, foreground, Globals.foregroundTilemap.WorldToCell(position));

    //
    // Summary:
    //      The method called whenever a block is placed.
    // Parameters:
    //      toPlace:
    //          The block type to place.
    //      foreground:
    //          Whether or not the block should be placed in the foreground.
    //      position:
    //          The position of the placed block
    public static void PlaceBlockCell(Block toPlace, bool foreground, Vector3Int tilePosition)
    {
        BlockTile newTile = BlockTile.CreateInstance<BlockTile>();
        newTile.sourceBlock = toPlace;
        newTile.sprite = toPlace.blockSprite;
        newTile.name = toPlace.blockName;

        if(foreground) {
            newTile.colliderType = Tile.ColliderType.Grid;
            Globals.foregroundTilemap.SetTile(tilePosition, newTile);
            currentWorld[tilePosition.x, tilePosition.y, 0] = toPlace.blockID + 1;
        } else {
            newTile.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            Globals.backgroundTilemap.SetTile(tilePosition, newTile);
            currentWorld[tilePosition.x, tilePosition.y, 1] = toPlace.blockID + 1;
        }
    }

    public static void PlaceBlockCell(Block toPlace, bool foreground, Vector2 tilePosition) => PlaceBlockCell(toPlace, foreground, new Vector3Int(Mathf.RoundToInt(tilePosition.x), Mathf.RoundToInt(tilePosition.y), 0));
}

public class BlockTile : Tile 
{
    public Block sourceBlock;

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
        foreach(Vector3Int dir in new List<Vector3Int>() {Vector3Int.up, Vector3Int.down, Vector3Int.right, Vector3Int.left}) {
            if(HasSmoothableTile(position + dir, tilemap)) {
                tilemap.RefreshTile(position + dir); // This doesn't actuall call this same method, but the followind GetTileData() method, so don't worry about infinite loops.
            }
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        if(!sourceBlock.blockSmoothing || (sourceBlock.spriteSheet == null)) { 
            base.GetTileData(position, tilemap, ref tileData);
            return; 
        } // If the tile doesn't or can't smooth, don't even try

        int bitmask = 0; // Using bitmask smoothing, look it up

        if(HasSmoothableTile(position + Vector3Int.up, tilemap)) {
            bitmask += 1;
        }
        if(HasSmoothableTile(position + Vector3Int.down, tilemap)) {
            bitmask += 2;
        }
        if(HasSmoothableTile(position + Vector3Int.right, tilemap)) {
            bitmask += 4;
        }
        if(HasSmoothableTile(position + Vector3Int.left, tilemap)) {
            bitmask += 8;
        }

        sprite = sourceBlock.spriteSheet.spritesDict[sourceBlock.blockSprite.texture.name + "_" + bitmask];
        base.GetTileData(position, tilemap, ref tileData);
    }

    private bool HasSmoothableTile(Vector3Int position, ITilemap tilemap) {
        return tilemap.GetTile(position) != null;
    }

}
