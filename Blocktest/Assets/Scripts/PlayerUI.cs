using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
/* TODO: Finish seperating the UI elements of BuildSystem.cs to here
public class PlayerUI : MonoBehaviour
{
    /// The linked build system component.
    private BuildSystem buildSystem;

    /// Maximum distance at which the player can place blocks
    [SerializeField] float maxBuildDistance = 3f;

    ///The block placement template object.
    private GameObject blockCursor;
    /// The block placement template's sprite renderer.
    private SpriteRenderer currentRenderer;
    /// The block placement template's audio source
    private AudioSource audioSource;

    /// Tilemap for foreground objects
    [SerializeField] Tilemap foregroundTilemap;
    ///Tilemap for background (non-dense) objects
    [SerializeField] Tilemap backgroundTilemap;

    // Start is called before the first frame update
    void Start()
    {
        buildSystem = GetComponent<BuildSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int tilePosition = foregroundTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Vector3 worldTilePosition = foregroundTilemap.CellToWorld(tilePosition) + foregroundTilemap.cellSize / 2;
        blockCursor.transform.position = foregroundTilemap.CellToWorld(tilePosition) + foregroundTilemap.cellSize / 2;

        if(buildSystem.buildMode) { //Things to happen in "build mode"
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if(scrollInput != 0)
            {
                // Change ID by -1 if scroll input is greater than zero, otherwise change ID by +1.
                buildSystem.CycleBlockSelection(scrollInput > 0 ? -1 : 1);
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
        } else { // Things to happen in "destroy mode"
            if(Input.GetMouseButton(0)) {
                buildSystem.BreakBlock(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
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
        if(!buildSystem.buildMode) {
            currentRenderer.sprite = destroySprite;
        }
    }

}
*/