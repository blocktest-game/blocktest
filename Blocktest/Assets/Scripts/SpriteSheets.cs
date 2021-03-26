using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[System.Serializable]
public class SpriteSheet
{
	public Dictionary<string, Sprite> spritesDict = new Dictionary<string, Sprite>();

	public SpriteSheet(string texturePath)
	{
		string path = texturePath.Replace("Assets/Resources/", null);
		Sprite[] sprites = Resources.LoadAll<Sprite>(path);
		foreach (Sprite sprite in sprites)
		{
			spritesDict[sprite.name] = sprite;
		}
	}

}