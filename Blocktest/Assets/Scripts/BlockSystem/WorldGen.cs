using Blocks;
using UnityEngine;

namespace BlockSystem
{
    public static class WorldGen
    {
        public static float stonePercentage = 80;
        /// <summary>
        /// The type of block used on the lowest layer.
        /// </summary>

        public static Block stoneBlock;
        /// <summary>
        /// The type of block used on the middle layer.
        /// </summary>
        public static Block dirtBlock;
        /// <summary>
        /// The type of block used on the topmost layer.
        /// </summary>
        public static Block grassBlock;

        /// <summary>
        /// The "intensity" of the generated area's elevation changes
        /// </summary>
        private static float intensity = 1.0f;

        public static void GenerateMainMap()
        {
            if(Globals.worldSeed == 0.0f) {
                Globals.worldSeed = Random.Range(0.0f, 1000000.0f);
            }
        
#if UNITY_EDITOR // Generates a much smaller map if you're in the editor for sanity reasons
            GenerateWorld(new Vector2Int(0, 0), new Vector2Int(128, 128), Globals.worldSeed);
#else
            GenerateWorld(new Vector2Int(0, 0), new Vector2Int(510, 255), Globals.worldSeed);
#endif
        }

        /// <summary>
        /// Generates terrain at the specified coordinates with the specified seed
        /// </summary>
        /// <param name="startLoc">The starting location for the generator.</param>
        /// <param name="endLoc">The ending location for the generator.</param>
        /// <param name="generatorSeed">The random number seed used to get the noise used by the generator. Deafult yields a random seed.</param>
        public static void GenerateWorld(Vector2Int startLoc, Vector2Int endLoc, float generatorSeed = 0.0f)
        {
            if (generatorSeed == 0.0f) {
                generatorSeed = Random.Range(0.0f, 1000000.0f);
            }

            dirtBlock = Block.Instance<Dirt>();
            grassBlock = Block.Instance<Grass>();
            stoneBlock = Block.Instance<Stone>();

            for (int xi = startLoc.x; xi < endLoc.x; xi++) {
                float x = ((float)xi + 1) / 10;
                float result = Mathf.PerlinNoise(x * intensity + generatorSeed, generatorSeed);
                int height = Mathf.RoundToInt(Mathf.Clamp01(result / 8 + 0.875f) * (endLoc.y / 2.0f)); // Gets the height of the column
                float stoneResult = Mathf.PerlinNoise(x * 2 * intensity + generatorSeed, generatorSeed);
                int stoneHeight = Mathf.RoundToInt((Mathf.Clamp01(stoneResult / 8 + 0.875f)) * (height * (stonePercentage / 100)));

                for (int yi = startLoc.y; yi < endLoc.y; yi++) {
                    Block toPlace;
                    if (yi < stoneHeight) {
                        toPlace = stoneBlock;
                    } else if (yi < height) {
                        toPlace = dirtBlock;
                    } else if (yi == height) {
                        toPlace = grassBlock;
                        if (xi == Mathf.RoundToInt(endLoc.x / 2.0f)) {
                            Globals.instance.InitializePlayer(Globals.instance.CellToWorld(new Vector3(xi, yi + 5, -10))); // TODO: Move this to globals somehow
                        }
                    } else {
                        continue;
                    }

                    BuildSystem.PlaceBlockCell(toPlace, true, new Vector2(xi, yi));
                    BuildSystem.PlaceBlockCell(toPlace, false, new Vector2(xi, yi));
                }
            }
        }

    }
}
