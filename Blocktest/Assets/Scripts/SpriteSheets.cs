using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The SpriteSheet class. See <see cref="spritesDict"/> for the sprites retrieved in the sprite sheet.
/// </summary>
[System.Serializable]
public class SpriteSheet
{
	/// <summary>The dictionary of subsprites retrieved from the texture, with the index being the name of the subsprite.</summary>
	public Dictionary<string, Sprite> spritesDict = new Dictionary<string, Sprite>();

	/// <summary>
	/// Create a Sprite Sheet from the path provided.
	/// </summary>
	/// <remarks>
	/// Sprite must be in the /Assets/Resources folder. Path will be from there, but if the provided path contains "/Assets/Resources" it will be removed.
	/// </remarks>
	/// <param name="texturePath"></param>
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