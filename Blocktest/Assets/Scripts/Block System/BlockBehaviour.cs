using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    /// The block's ID (index in the allblocks list). Leave as -1 for automatic assignment based on init order (probably not a good idea)
    public int blockID = -1;
    /// The block's name.
    public string blockName = "Error";

    /// Whether or not the block supports icon smoothing.
    public bool blockSmoothing = false;
    /// Whether or not a block smooths only with itself
    public bool smoothSelf = false;

    /// Whether or not a block can be placed in the background.
    public bool canPlaceBackground = true;
    /// The block's sprite.
    public Sprite blockSprite;
    /// The sound that is played when the block is placed.
    public AudioClip placeSound;
    /// The sprite sheet used for smoothing the block.
    public SpriteSheet spriteSheet;

    /* METHODS */

    public virtual void Initialize(){
        string path = "Sprites/Blocks/" + blockName.ToLower().Replace(" ", null);
        blockSprite = Resources.Load<Sprite>(path);
        if(blockSprite == null) {
            Debug.Log("Block " + this + " does not have an icon at " + path + "!");
        }
        if(blockSmoothing) {
            spriteSheet = new SpriteSheet(path);
            if(spriteSheet.spritesDict.Count <= 1) {
                Debug.Log("Block " + this + " is marked as smoothable, but a sprite sheet could not be found at " + path + "!");
            }
        }
    }

    // Called whenever a block is placed
    public virtual void OnPlace(Vector3Int position, bool foreground) {

    }
    // Called whenever a block is broken
    public virtual void OnBreak(Vector3Int position, bool foreground){

    }

}
