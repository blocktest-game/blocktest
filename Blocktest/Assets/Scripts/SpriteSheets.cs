using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class SpriteSheet
{
	public Texture2D Texture;

	public Dictionary<string, Sprite> spritesDict = new Dictionary<string, Sprite>();

	public SpriteSheet(Texture2D texture)
	{
		Texture = texture;
		string path = AssetDatabase.GetAssetPath(Texture).Replace("Assets/Resources/", null);
		Sprite[] sprites = Resources.LoadAll<Sprite>(path.Remove(path.Length - 4));
		foreach (Sprite sprite in sprites)
		{
			spritesDict[sprite.name] = sprite;
		}
	}

	public SpriteSheet(string texturePath)
	{
		string path = texturePath.Replace("Assets/Resources/", null);
		Sprite[] sprites = Resources.LoadAll<Sprite>(path.Remove(path.Length - 4));
		foreach (Sprite sprite in sprites)
		{
			spritesDict[sprite.name] = sprite;
		}
	}

}