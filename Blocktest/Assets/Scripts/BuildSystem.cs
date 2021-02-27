using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuildSystem : MonoBehaviour
{

    /// Reference to main BlockManager script.
    private BlockManager blockManager;
    /// Reference to the player object
    [SerializeField] GameObject playerObject;

    /// The ID of the currently selected block.
    private int currentBlockID = 0;
    /// The currently selected block.
    private Block currentBlock;

    ///The block placement template object.
    private GameObject blockCursor;
    /// The block placement template's sprite renderer.
    private SpriteRenderer currentRenderer;
    /// The block placement template's audio source
    private AudioSource audioSource;

    /// The sprite to show around the cursor when in destroy mode.
    [SerializeField] Sprite destroySprite;


    /// Whether build mode is on or not.
    public bool buildMode = false;


    /// Maximum distance at which the player can place blocks
    [SerializeField] float maxBuildDistance = 3f;

    /// Tilemap for foreground objects
    [SerializeField] Tilemap foregroundTilemap;
    ///Tilemap for background (non-dense) objects
    [SerializeField] Tilemap backgroundTilemap;

    /// Dropdown used for item selection
    [SerializeField] Dropdown selectionDropdown;

    // Start is called before the first frame update
    void Start()
    {
        blockManager = GetComponent<BlockManager>();
        InitializeCursor();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("e"))
        {
            ToggleBuild();
        }

        Vector3Int tilePosition = foregroundTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Vector3 worldTilePosition = foregroundTilemap.CellToWorld(tilePosition) + foregroundTilemap.cellSize / 2;
        blockCursor.transform.position = foregroundTilemap.CellToWorld(tilePosition) + foregroundTilemap.cellSize / 2;

        if(buildMode) 
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if(scrollInput != 0)
            {
                // Change ID by -1 if scroll input is greater than zero, otherwise change ID by +1.
                CycleBlockSelection(scrollInput > 0 ? -1 : 1);
            }

            currentRenderer.sprite = currentBlock.blockSprite;

            bool canBuildForeground = foregroundTilemap.GetTile(tilePosition) == null;
            bool canBuildBackground = backgroundTilemap.GetTile(tilePosition) == null;

            if(canBuildForeground) {
                if(Physics2D.BoxCast(worldTilePosition, foregroundTilemap.cellSize / 2, 0, Vector2.zero).collider != null){
                    canBuildForeground = false;
                }
            }

            if(Vector2.Distance(worldTilePosition, playerObject.transform.position) > maxBuildDistance) {
                canBuildBackground = false;
                canBuildForeground = false;
            }

            if(!canBuildForeground) {
                currentRenderer.color = new Color(1f, 0f, 0f, 0.7f); // Red if you can't build on the foreground
            } else if(!canBuildBackground) {
                currentRenderer.color = new Color(0f, 0f, 1f, 0.7f); // Blue if you can't build on the background, but can build in the foreground
            } else {
                currentRenderer.color = new Color(0.5f, 1f, 0.5f, 0.7f); // Otherwise, normal coloring
            }

            if (canBuildForeground && Input.GetMouseButton(0))
            {
                PlaceBlock(currentBlock, true, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            if (canBuildBackground && Input.GetMouseButton(1))
            {
                PlaceBlock(currentBlock, false, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }

        }

        if(!buildMode && Input.GetMouseButton(0))
        {
            BreakBlock(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

    }

    public void ToggleBuild()
    {
        buildMode = !buildMode;
        if (blockCursor == null)
        {
            InitializeCursor();
        }

        //Set the current block.
        if (currentBlock == null)
        {
            //Ensure the block ID is valid.
            if (blockManager.allBlocks[currentBlockID] != null)
            {
                currentBlock = blockManager.allBlocks[currentBlockID];
            }
        }
        currentRenderer.color = new Color(1f, 1f, 1f, 1f);
        if(buildMode) {
            currentRenderer.sprite = currentBlock.blockSprite;
        } else {
            currentRenderer.sprite = destroySprite;
        }
    }

    private void InitializeCursor()
    {
        if(blockCursor) {
            Destroy(blockCursor);
        }
        blockCursor = new GameObject("BlockCursor");
        currentRenderer = blockCursor.AddComponent<SpriteRenderer>();
        audioSource = blockCursor.AddComponent<AudioSource>();
        if(!buildMode) {
            currentRenderer.sprite = destroySprite;
        }
        if (currentBlock == null) {
            //Ensure the block ID is valid.
            if (blockManager.allBlocks[currentBlockID] != null)
            {
                currentBlock = blockManager.allBlocks[currentBlockID];
            }
        }
    }

    public void BreakBlock(Vector2 position)
    {
        Vector3Int tilePosition = foregroundTilemap.WorldToCell(position);
        if(foregroundTilemap.HasTile(tilePosition)) {
            foregroundTilemap.SetTile(tilePosition, null);
        } else if (backgroundTilemap.HasTile(tilePosition)) {
            backgroundTilemap.SetTile(tilePosition, null);
        }
    }

    public void PlaceBlock(Block toPlace, bool foreground, Vector2 position)
    {
        Vector3Int tilePosition = foregroundTilemap.WorldToCell(position);
        BlockTile newTile = BlockTile.CreateInstance<BlockTile>();
        newTile.sourceBlock = toPlace;
        newTile.sprite = toPlace.blockSprite;
        newTile.name = toPlace.blockName;
        audioSource.PlayOneShot(currentBlock.placeSound);

        if(foreground) {
            newTile.colliderType = Tile.ColliderType.Grid;
            Debug.Log(tilePosition);
            foregroundTilemap.SetTile(tilePosition, newTile);
        } else {
            newTile.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            backgroundTilemap.SetTile(tilePosition, newTile);
        }
    }

    public void CycleBlockSelection(int slotDelta)
    {
        int totalBlocks = blockManager.allBlocks.Length - 1;
        int newBlockID = currentBlockID + slotDelta;
        if(newBlockID > totalBlocks) {
            newBlockID = 0;
        } else if(newBlockID < 0) {
            newBlockID = totalBlocks;
        }
        ChangeBlockSelection(newBlockID);
    }

    public void ChangeBlockSelection(int slot)
    {
        slot = Mathf.Clamp(slot, 0, blockManager.allBlocks.Length - 1);
        currentBlockID = slot;
        currentBlock = blockManager.allBlocks[currentBlockID];
        selectionDropdown.captionText.text = currentBlock.blockName;
        if(buildMode) {
            currentRenderer.sprite = currentBlock.blockSprite;
        }
    }
}

public class BlockTile : Tile 
{
    public Block sourceBlock;
}
