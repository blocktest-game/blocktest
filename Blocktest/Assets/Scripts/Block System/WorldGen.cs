using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGen : MonoBehaviour
{
    [SerializeField] Tilemap foregroundTilemap;
    [SerializeField] Tilemap backgroundTilemap;
    [SerializeField] BlockManager blockManager;
    [SerializeField] BuildSystem buildSystem;

    [SerializeField] int stonePercentage;
    /// The type of block used on the lowest layer.

    public Block stoneBlock;
    /// The type of block used on the middle layer.
    public Block dirtBlock;
    /// The type of block used on the topmost layer.
    public Block grassBlock;

    /// The terrain seed used.
    public float worldSeed;
    /// The "intensity" of the generated area's elevation changes
    [SerializeField] float intensity = 10.0f;
    /// Progress of the generator on the generation

    public float progress = 0.0f;
    /// The spawn platform.
    [SerializeField] GameObject spawnPlatform;

    void Start()
    {
        blockManager = GetComponent<BlockManager>();
        buildSystem = GetComponent<BuildSystem>();

        worldSeed = Random.Range(0.0f, 1000000.0f);
        Generate(255, 255, worldSeed);
    }

    public void Generate(int maxX = 255, int maxY = 255, float generatorSeed = 0.0f) 
    {
        if(generatorSeed == 0.0f) {
            generatorSeed = Random.Range(0.0f, 1000000.0f);
        }

        dirtBlock = blockManager.allBlocks[0]; // TODO: Find a way to dynamically reserve certain blocks
        grassBlock = blockManager.allBlocks[1];
        stoneBlock = blockManager.allBlocks[2];

        progress = 0.0f;
        
        for (int xi = 0; xi < maxX; xi++) {
            float x = (float)xi / 10;
            float result = Mathf.PerlinNoise(x * intensity + generatorSeed, generatorSeed);
            int height = Mathf.RoundToInt(result * maxY); // Gets the height of the column
            int stoneHeight = Mathf.RoundToInt((float)height * ((float)stonePercentage / 100));

            for (int yi = 0; yi < maxY; yi++) {
                Block toPlace;
                if(yi < stoneHeight) {
                    toPlace = stoneBlock;
                }
                else if(yi < height) {
                    toPlace = dirtBlock;
                }
                else if(yi == height) {
                    toPlace = grassBlock;
                    if(xi == Mathf.RoundToInt(maxX / 2)) {
                        spawnPlatform.transform.position = foregroundTilemap.CellToWorld(new Vector3Int(xi, yi + 1, 0));
                        GameObject.Find("Player").transform.position = foregroundTilemap.CellToWorld(new Vector3Int(xi, yi + 5, 0));
                    }
                }
                else {
                    continue;
                }

                buildSystem.PlaceBlockCell(toPlace, true, new Vector2(xi, yi));
                buildSystem.PlaceBlockCell(toPlace, false, new Vector2(xi, yi));
                //progress += 1.0f / ((float)maxX + (float)maxY); // Add progress to the progress var
            }
        }
    }

}
