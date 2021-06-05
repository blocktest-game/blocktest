using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The SpriteSheet class. See <see cref="spritesDict"/> for the sprites retrieved in the sprite sheet.
/// </summary>
[Serializable]
public class SpriteSheet : IEquatable<SpriteSheet>
{
    /// <summary>The dictionary of subsprites retrieved from the texture, with the index being the name of the subsprite.</summary>
    private readonly Dictionary<string, Sprite> spritesDict = new Dictionary<string, Sprite>();
    private readonly Sprite[] sprites;
    public int Length => sprites.Length;

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
        sprites = Resources.LoadAll<Sprite>(path);
        foreach (Sprite sprite in sprites) {
            spritesDict[sprite.name] = sprite;
        }
    }

    public Sprite this[int index] { get => sprites[index]; }
    public Sprite this[string name] { get => spritesDict[name]; }
    
    public bool Equals(SpriteSheet other)
    {
        return other is { } && (ReferenceEquals(this, other) || Equals(sprites, other.sprites));
    }
    
    public override int GetHashCode()
    {
        return sprites.Sum(sprite => sprite.GetHashCode());
    }
}