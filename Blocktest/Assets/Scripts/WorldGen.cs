using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGen : MonoBehaviour
{
    [SerializeField] int maxX = 255;
    [SerializeField] int maxY = 255;
    [SerializeField] Tilemap foregroundTilemap;
    [SerializeField] BlockManager blockManager;
    public Block stoneBlock;
    public Block dirtBlock;
    public Block grassBlock;
    // Start is called before the first frame update
    void Start()
    {
        // TODO: use Mathf.PerlinNoise()
        dirtBlock = blockManager.allBlocks[0]; // TODO: Find a way to dynamically reserve certain blocks
        grassBlock = blockManager.allBlocks[1];
        stoneBlock = blockManager.allBlocks[2];
        for (int xi = 0; xi < maxX; xi++) {
            for (int yi = 0; yi < maxY; yi++) {
                Block toPlace;
                if(yi < 30) {
                    toPlace = stoneBlock;
                }
                else if(yi < 40) {
                    toPlace = dirtBlock;
                }
                else if(yi == 40) {
                    toPlace = grassBlock;
                }
                else {
                    continue;
                }

                BlockTile newTile = BlockTile.CreateInstance<BlockTile>();
                newTile.sourceBlock = toPlace;
                newTile.sprite = toPlace.blockSprite;
                newTile.name = toPlace.blockName;
                foregroundTilemap.SetTile(new Vector3Int(xi, yi, 0), newTile);
            }
        }
    }

}
