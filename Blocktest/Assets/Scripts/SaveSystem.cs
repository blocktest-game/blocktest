using BlockSystem;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

static class SaveSystem
{
    public static void SaveGame(int saveIndex) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savegame-" + saveIndex + ".bt";

        FileStream stream = new FileStream(path, FileMode.Create);
        Vector3 playerPos3 = Globals.characterObject.transform.position;
        float[] playerPos = new float[] {playerPos3.x, playerPos3.y, playerPos3.z};

        SaveData save = new SaveData(playerPos, BuildSystem.currentWorld);

        formatter.Serialize(stream, save);
        stream.Close();
    }

    public static void LoadGame(int saveIndex) {
        string path = Application.persistentDataPath + "/savegame-" + saveIndex + ".bt";

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;

            stream.Close();
            
            if (data is null) { return; }

            int[] blocksToPlaceFore = new int[Globals.instance.maxX * Globals.instance.maxY];
            int[] blocksToPlaceBack = new int[Globals.instance.maxX * Globals.instance.maxY];
            Vector3Int[] positions = new Vector3Int[Globals.instance.maxX * Globals.instance.maxY];
            for (int x = 0; x < Globals.instance.maxX; x++) {
                for (int y = 0; y < Globals.instance.maxY; y++) {
                    Globals.instance.foregroundTilemap.ClearAllTiles();
                    Globals.instance.backgroundTilemap.ClearAllTiles();
                    int i = x * Globals.instance.maxY + y;
                    positions[i] = new Vector3Int(x, y, 0);
                    if(data.worldData[x, y, 0] != 0) {blocksToPlaceFore[i] = data.worldData[x, y, 0];}
                    if(data.worldData[x, y, 1] != 0) {blocksToPlaceBack[i] = data.worldData[x, y, 1];}
                }
            }

            BuildSystem.PlaceIDsCells(blocksToPlaceFore, true, positions);
            BuildSystem.PlaceIDsCells(blocksToPlaceBack, false, positions);
            Globals.characterObject.transform.position = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);
            BuildSystem.currentWorld = data.worldData;

        } else
        {
            Debug.LogError("Error: Save file not found in " + path);
        }
    }
}

[Serializable]
public class SaveData
{
    public DateTime createDate;
    public DateTime saveDate;
    public float[] playerPosition;
    public int[,,] worldData;
    public SaveData(float[] playerPosition, int[,,] worldData)
    {
        this.playerPosition = playerPosition;
        this.worldData = worldData;
        this.saveDate = DateTime.Now;
    }
}
