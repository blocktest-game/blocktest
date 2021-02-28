using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuildSystem : MonoBehaviour
{
    /// Tilemap for foreground objects
    [SerializeField] Tilemap foregroundTilemap;
    /// Tilemap for background (non-dense) objects
    [SerializeField] Tilemap backgroundTilemap;

    //
    // Summary:
    //      The method called whenever an object is removed.
    // Parameters:
    //      foreground:
    //          Whether or not the block to be destroyed is in the foreground.
    //      position:
    //          The position of the block to destroy (world coords)
    public void BreakBlockWorld(bool foreground, Vector2 position)
    {
        BreakBlockCell(foreground, foregroundTilemap.WorldToCell(position));
    }

    public void BreakBlockCell(bool foreground, Vector3Int tilePosition)
    {
        if(foreground && foregroundTilemap.HasTile(tilePosition)) {
            foregroundTilemap.SetTile(tilePosition, null);
        } else if (!foreground && backgroundTilemap.HasTile(tilePosition)) {
            backgroundTilemap.SetTile(tilePosition, null);
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
    public void PlaceBlockWorld(Block toPlace, bool foreground, Vector2 position)
    {
        PlaceBlockCell(toPlace, foreground, foregroundTilemap.WorldToCell(position));
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
    public void PlaceBlockCell(Block toPlace, bool foreground, Vector3Int tilePosition)
    {
        BlockTile newTile = BlockTile.CreateInstance<BlockTile>();
        newTile.sourceBlock = toPlace;
        newTile.sprite = toPlace.blockSprite;
        newTile.name = toPlace.blockName;

        if(foreground) {
            newTile.colliderType = Tile.ColliderType.Grid;
            foregroundTilemap.SetTile(tilePosition, newTile);
        } else {
            newTile.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            backgroundTilemap.SetTile(tilePosition, newTile);
        }
    }

    public void PlaceBlockCell(Block toPlace, bool foreground, Vector2 tilePosition)
    {
        PlaceBlockCell(toPlace, foreground, new Vector3Int(Mathf.RoundToInt(tilePosition.x), Mathf.RoundToInt(tilePosition.y), 0));
    }
}

public class BlockTile : Tile 
{
    public Block sourceBlock;
}
