using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public static class BuildSystem
{

    [SerializeField] public static int[,,] currentWorld = new int[Globals.maxX, Globals.maxY, 2];

    //
    // Summary:
    //      The method called whenever an object is removed.
    // Parameters:
    //      foreground:
    //          Whether or not the block to be destroyed is in the foreground.
    //      position:
    //          The position of the block to destroy (world coords)
    public static void BreakBlockWorld(bool foreground, Vector2 position) => BreakBlockCell(foreground, Globals.foregroundTilemap.WorldToCell(position));

    public static void BreakBlockCell(bool foreground, Vector3Int tilePosition)
    {
        if(foreground && Globals.foregroundTilemap.HasTile(tilePosition)) {
            Globals.foregroundTilemap.SetTile(tilePosition, null);
            currentWorld[tilePosition.x, tilePosition.y, 0] = 0;
        } else if (!foreground && Globals.backgroundTilemap.HasTile(tilePosition)) {
            Globals.backgroundTilemap.SetTile(tilePosition, null);
            currentWorld[tilePosition.x, tilePosition.y, 1] = 0;
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
}
