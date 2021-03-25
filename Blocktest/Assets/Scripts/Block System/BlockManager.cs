// System required for [Serializable] attribute.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BlockManager : MonoBehaviour {

    /// Array we expose to inspector / editor, use this instead of the old arrays to define block types.
    [SerializeField] BlockType[] allBlockTypes;

    /// Array to store all blocks created in Start()
    [HideInInspector] public Block[] allBlocks;
    /// List used to store the names of blocks, the index is the corresponding ID.
    [HideInInspector] public List<string> blockNames;

    /// Dropdown used for item selection
    [SerializeField] Dropdown selectionDropdown;

    /// Tilemap for foreground objects
    [SerializeField] Tilemap foregroundTilemap;
    /// Tilemap for background (non-dense) objects
    [SerializeField] Tilemap backgroundTilemap;


    private void Awake()
    {
        // Add this to the global variable
        Globals.blockManager = this;
        Globals.foregroundTilemap = foregroundTilemap;
        Globals.backgroundTilemap = backgroundTilemap;

        // Initialise allBlocks array.
        allBlocks = new Block[allBlockTypes.Length];
        
        // For loops to populate main allBlocks array.
        for (int i = 0; i < allBlockTypes.Length; i++)
        {
            // Instead of referencing multiple arrays, we just create a new BlockType object and get values from that.
            BlockType newBlockType = allBlockTypes[i];
            allBlocks[i] = new Block(i, newBlockType.blockName, newBlockType.blockSprite, newBlockType.placeSound, newBlockType.blockSmoothing);
            blockNames.Add(newBlockType.blockName);
        }
        selectionDropdown.AddOptions(blockNames);
        WorldGen.GenerateMainMap();
    }
}

// We still use the Block class to store the final Block type data.
public class Block
{
    /// The block's ID (index in the allblocks list)
    public int blockID;
    /// The block's name.
    public string blockName;
    /// Whether or not the block supports icon smoothing.
    public bool blockSmoothing = false;
    /// The block's sprite.
    public Sprite blockSprite;
    /// The sound that is played when the block is placed.
    public AudioClip placeSound;
    /// The sprite sheet used for smoothing the block.
    public SpriteSheet spriteSheet;

    public Block(int id, string name, Sprite sprite, AudioClip place, bool smooth)
    {
        blockID = id;
        blockName = name;
        blockSprite = sprite;
        placeSound = place;
        blockSmoothing = smooth;
        if(smooth) {
            spriteSheet = new SpriteSheet("Sprites/" + blockSprite.texture.name);
        }
    }
}

// Custom struct for Block type.
[Serializable]
public struct BlockType
{
    // Main, differing variables for each block type.
    public string blockName;
    public Sprite blockSprite;
    public AudioClip placeSound;
    public bool blockSmoothing;
}
