// System required for [Serializable] attribute.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour {

    /// Array we expose to inspector / editor, use this instead of the old arrays to define block types.
    [SerializeField] BlockType[] allBlockTypes;

    /// Array to store all blocks created in Start()
    [HideInInspector] public Block[] allBlocks;
    /// List used to store the names of blocks, the index is the corresponding ID.
    [HideInInspector] public List<string> blockNames;

    /// Dropdown used for item selection
    [SerializeField] Dropdown selectionDropdown;


    private void Awake()
    {
        // Initialise allBlocks array.
        allBlocks = new Block[allBlockTypes.Length];
        
        // For loops to populate main allBlocks array.
        for (int i = 0; i < allBlockTypes.Length; i++)
        {
            // Instead of referencing multiple arrays, we just create a new BlockType object and get values from that.
            BlockType newBlockType = allBlockTypes[i];
            allBlocks[i] = new Block(i, newBlockType.blockName, newBlockType.blockSprite, newBlockType.placeSound);
            blockNames.Add(newBlockType.blockName);
        }
        selectionDropdown.AddOptions(blockNames);
    }
}

// We still use the Block class to store the final Block type data.
public class Block
{
    /// The block's ID (index in the allblocks list)
    public int blockID;
    /// The block's name.
    public string blockName;
    /// The block's sprite.
    public Sprite blockSprite;
    /// The sound that is played when the block is placed.
    public AudioClip placeSound;

    public Block(int id, string myName, Sprite mySprite, AudioClip place)
    {
        blockID = id;
        blockName = myName;
        blockSprite = mySprite;
        placeSound = place;
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
}
