using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldGen
{
    public static float stonePercentage = 80;
    /// The type of block used on the lowest layer.

    public static Block stoneBlock;
    /// The type of block used on the middle layer.
    public static Block dirtBlock;
    /// The type of block used on the topmost layer.
    public static Block grassBlock;

    /// The terrain seed used.
    public static float worldSeed;
    /// The "intensity" of the generated area's elevation changes
    static float intensity = 1.0f;
    /// Progress of the generator on the generation
    public static float progress = 0.0f;

    public static void GenerateMainMap()
    {
        worldSeed = Random.Range(0.0f, 1000000.0f);
#if UNITY_EDITOR // Generates a much smaller map if you're in the editor for sanity reasons
        GenerateWorld(new Vector2Int(0, 0), new Vector2Int(128, 128), worldSeed);
#else
        GenerateWorld(new Vector2Int(0, 0), new Vector2Int(510, 255), worldSeed);
#endif
    }

    public static void GenerateWorld(Vector2Int startLoc, Vector2Int endLoc, float generatorSeed = 0.0f) 
    {
        if(generatorSeed == 0.0f) {
            generatorSeed = Random.Range(0.0f, 1000000.0f);
        }

        dirtBlock = Globals.allBlocks[0]; // TODO: Find a way to dynamically reserve certain blocks
        grassBlock = Globals.allBlocks[1];
        stoneBlock = Globals.allBlocks[2];

        progress = 0.0f;
        
        for (int xi = startLoc.x; xi < endLoc.x; xi++) {
            float x = ((float)xi + 1) / 10;
            float result = Mathf.PerlinNoise(x * intensity + generatorSeed, generatorSeed);
            int height = Mathf.RoundToInt(Mathf.Clamp01(result / 8 + 0.875f) * (endLoc.y / 2)); // Gets the height of the column
            float stoneResult = Mathf.PerlinNoise(x * 2 * intensity + generatorSeed, generatorSeed);
            int stoneHeight = Mathf.RoundToInt((Mathf.Clamp01(stoneResult / 8 + 0.875f)) * ((float)height * (stonePercentage / 100)));

            for (int yi = startLoc.y; yi < endLoc.y; yi++) {
                Block toPlace;
                if(yi < stoneHeight) {
                    toPlace = stoneBlock;
                }
                else if(yi < height) {
                    toPlace = dirtBlock;
                }
                else if(yi == height) {
                    toPlace = grassBlock;
                    if(xi == Mathf.RoundToInt(endLoc.x / 2)) {
                        GameObject.Find("Player").transform.position += Globals.CellToWorld(new Vector2(xi, yi + 5));
                    }
                }
                else {
                    continue;
                }

                BuildSystem.PlaceBlockCell(toPlace, true, new Vector2(xi, yi));
                BuildSystem.PlaceBlockCell(toPlace, false, new Vector2(xi, yi));
                //progress += 1.0f / ((float)maxX + (float)maxY); // Add progress to the progress var
            }
        }
    }

}
