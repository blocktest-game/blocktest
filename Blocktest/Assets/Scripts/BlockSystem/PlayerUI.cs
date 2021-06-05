using UnityEngine;

namespace BlockSystem
{
    public class PlayerUI : MonoBehaviour
    {
        /// <summary> The ID of the currently selected block. </summary>
        private int currentBlockID;
        /// <summary> The currently selected block. </summary>
        private Block currentBlock;

        /// <summary> The block placement template object. </summary>
        private GameObject blockCursor;
        /// <summary> The block placement template's sprite renderer. </summary>
        private SpriteRenderer currentRenderer;
        /// <summary> The block placement template's audio source </summary>
        private AudioSource audioSource;
        /// <summary> The sprite to show around the cursor when in destroy mode. </summary>
        [SerializeField] private Sprite destroySprite;

        /// <summary> Whether build mode is on or not. </summary>
        public bool buildMode;

        /// <summary> Maximum distance at which the player can place blocks </summary>
        [SerializeField] private float maxBuildDistance = 5f;
        /// <summary> The main camera of the scene, retrieved in Start() to lower performance costs. </summary>
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
            InitializeCursor();
        }

        private void Update()
        {
            if (Input.GetKeyDown("e")) {
                ToggleBuild();
            }
            if(Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)) {
                PlayerSaveLevel(0);
            }

            Vector3Int tilePosition = Globals.instance.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            Vector3 worldTilePosition = Globals.instance.CellToWorld(tilePosition) + Globals.instance.foregroundTilemap.cellSize / 2;
            blockCursor.transform.position = Globals.instance.CellToWorld(tilePosition) + Globals.instance.foregroundTilemap.cellSize / 2;

            if (Vector2.Distance(worldTilePosition, gameObject.transform.position) > maxBuildDistance) {
                currentRenderer.color = new Color(1f, 0f, 0f, 0.7f);
                return;
            }

            if (buildMode) {
                float scrollInput = Input.GetAxis("Mouse ScrollWheel");
                if (scrollInput != 0) {
                    // Change ID by -1 if scroll input is greater than zero, otherwise change ID by +1.
                    CycleBlockSelection(scrollInput > 0 ? -1 : 1);
                }

                currentRenderer.sprite = currentBlock.blockSprite;

                bool canBuildForeground = Globals.instance.GetTile(tilePosition, true) is null;
                bool canBuildBackground = Globals.instance.GetTile(tilePosition, false) is null;

                if (canBuildForeground) {
                    if (Physics2D.BoxCast(worldTilePosition, Globals.instance.foregroundTilemap.cellSize / 2, 0, Vector2.zero).collider is { }) {
                        canBuildForeground = false;
                    }
                }

                if (!canBuildForeground) {
                    currentRenderer.color = new Color(1f, 0f, 0f, 0.7f); // Red if you can't build on the foreground
                } else if (!canBuildBackground) {
                    currentRenderer.color = new Color(0f, 0f, 1f, 0.7f); // Blue if you can't build on the background, but can build in the foreground
                } else {
                    currentRenderer.color = new Color(0.5f, 1f, 0.5f, 0.7f); // Otherwise, normal coloring
                }

                if (canBuildForeground && Input.GetMouseButton(0)) {
                    PlayerPlaceBlock(currentBlock, true, mainCamera.ScreenToWorldPoint(Input.mousePosition));
                } else if (canBuildBackground && Input.GetMouseButton(1)) {
                    PlayerPlaceBlock(currentBlock, false, mainCamera.ScreenToWorldPoint(Input.mousePosition));
                }

            } else {
                if (Input.GetMouseButton(0)) {
                    PlayerBreakBlock(true, mainCamera.ScreenToWorldPoint(Input.mousePosition));
                } else if (Input.GetMouseButton(1)) {
                    PlayerBreakBlock(false, mainCamera.ScreenToWorldPoint(Input.mousePosition));
                }

            }

        }

        /// <summary>
        /// Initialize the cursor block template.
        /// </summary>
        private void InitializeCursor()
        {
            if (blockCursor) {
                Destroy(blockCursor);
            }
            blockCursor = new GameObject("BlockCursor");
            currentRenderer = blockCursor.AddComponent<SpriteRenderer>();
            audioSource = blockCursor.AddComponent<AudioSource>();
            currentRenderer.sortingOrder = 10;
            if (!buildMode) {
                currentRenderer.sprite = destroySprite;
            }
            if (currentBlock is null) {
                //Ensure the block ID is valid.
                if (Globals.instance.AllBlocks[currentBlockID] is { }) {
                    currentBlock = Globals.instance.AllBlocks[currentBlockID];
                }
            }
        }

        /// <summary>
        /// Toggle build mode on and off.
        /// </summary>
        public void ToggleBuild()
        {
            buildMode = !buildMode;
            if (blockCursor is null) {
                InitializeCursor();
            }

            //Set the current block.
            if (currentBlock is null) {
                //Ensure the block ID is valid.
                if (Globals.instance.AllBlocks[currentBlockID] is { }) {
                    currentBlock = Globals.instance.AllBlocks[currentBlockID];
                }
            }
            currentRenderer.color = new Color(1f, 1f, 1f, 1f);
            if (currentBlock is { }) {
                currentRenderer.sprite = buildMode ? currentBlock.blockSprite : destroySprite;
            }
        }

        /// <summary>
        /// Change the player's currently selected block by "cycling" a certain amount of slots.
        /// </summary>
        /// <param name="slotDelta">The amount of slots to "cycle."</param>
        public void CycleBlockSelection(int slotDelta)
        {
            int totalBlocks = Globals.instance.AllBlocks.Length - 1;
            int newBlockID = currentBlockID + slotDelta;
            if (newBlockID > totalBlocks) {
                newBlockID = 0;
            } else if (newBlockID < 0) {
                newBlockID = totalBlocks;
            }
            ChangeBlockSelection(newBlockID);
        }

        /// <summary>
        /// Change the player's currently selected block.
        /// </summary>
        /// <param name="slot">The slot to change to.</param>
        public void ChangeBlockSelection(int slot)
        {
            slot = Mathf.Clamp(slot, 0, Globals.instance.AllBlocks.Length - 1);
            currentBlockID = slot;
            currentBlock = Globals.instance.AllBlocks[currentBlockID];
            if (buildMode) {
                currentRenderer.sprite = currentBlock.blockSprite;
            }
        }

        /// <summary>
        /// The method called whenever a PLAYER places an object.
        /// </summary>
        /// <param name="toPlace">The block type to place.</param>
        /// <param name="foreground">Whether or not the block should be placed in the foreground.</param>
        /// <param name="position">The position of the placed block (world coords)</param>
        private void PlayerPlaceBlock(Block toPlace, bool foreground, Vector2 position)
        {
            if (toPlace.placeSound is { }) {
                audioSource.PlayOneShot(toPlace.placeSound);
            }
            BuildSystem.PlaceBlockWorld(toPlace, foreground, position);
        }

        /// <summary>
        /// The method called whenever a PLAYER tries to break an object.
        /// </summary>
        /// <param name="foreground">Whether or not the block to be destroyed is in the foreground.</param>
        /// <param name="position">The position of the block to destroy (world coords)</param>
        private void PlayerBreakBlock(bool foreground, Vector2 position)
        {
            BuildSystem.BreakBlockWorld(foreground, position);
        }

        public void PlayerLoadLevel(int saveIndex) {
            SaveSystem.LoadGame(saveIndex);
        }

        public void PlayerSaveLevel(int saveIndex) {
            SaveSystem.SaveGame(saveIndex);
        }

    }
}