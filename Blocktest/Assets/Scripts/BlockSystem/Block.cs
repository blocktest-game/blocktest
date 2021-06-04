using UnityEngine;

namespace BlockSystem
{
    public class Block
    {
        /// <summary> The block's ID (index in the allblocks list). </summary>
        /// <remarks> Leave as -1 for automatic assignment based on init order (probably not a good idea) </remarks>
        public int blockID = -1;
        /// <summary> The block's name. </summary>
        public string blockName = "Error";

        /// <summary> Whether or not the block supports icon smoothing. </summary>
        public bool blockSmoothing = false;
        /// <summary> Whether or not a block smooths only with itself </summary>
        /// <remarks> (Use normal 8x8 sprites to prevent overlap) </remarks>
        public bool smoothSelf = false;
        /// <summary> Whether or not a block can be placed in the background. </summary>
        public bool canPlaceBackground = true;

        /// <summary> The block's sprite. </summary>
        public Sprite blockSprite;
        /// <summary> The sound that is played when the block is placed. </summary>
        public AudioClip placeSound;
        /// <summary> The sprite sheet used for smoothing the block. </summary>
        public SpriteSheet spriteSheet;

        /* METHODS */

        /// <summary>
        /// Called whenever a block is first loaded by the block manager.
        /// </summary>
        /// <remarks>
        /// DO NOT FORGET TO CALL THE BASE METHOD IF YOU OVERRIDE THIS.
        /// </remarks>
        public virtual void Initialize()
        {
            string path = "Sprites/Blocks/" + blockName.ToLower().Replace(" ", null);
            blockSprite = Resources.Load<Sprite>(path);
            if (blockSprite == null) {
                Debug.Log("Block " + this + " does not have an icon at " + path + "!");
            }
            if (!blockSmoothing) { return; }
            spriteSheet = new SpriteSheet(path);
            if (spriteSheet.spritesDict.Count <= 1) {
                Debug.Log("Block " + this + " is marked as smoothable, but a sprite sheet could not be found at " + path + "!");
            }
        }

        /// <summary>
        /// Called whenever a block is placed.
        /// </summary>
        /// <param name="position">The position of the block being placed.</param>
        /// <param name="foreground">Whether the block being placed is in the foreground or not.</param>
        public virtual void OnPlace(Vector3Int position, bool foreground)
        {

        }
        /// <summary>
        /// Called whenever a block is broken.
        /// </summary>
        /// <param name="position">The position of the block being broken.</param>
        /// <param name="foreground">Whether the block being broken is in the foreground or not.</param>
        public virtual void OnBreak(Vector3Int position, bool foreground)
        {

        }

    }
}
