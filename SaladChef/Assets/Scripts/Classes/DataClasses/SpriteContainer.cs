using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpriteContiner", menuName = "ResourceContainers/SpriteContiner", order = 1)]
public class SpriteContainer : ScriptableObject
{
	[SerializeField]
	private Sprite[] sprites;
	private Dictionary<string, Sprite> spritesLookup;

	private void Awake()
	{
		if (sprites.Length > 0)
		{
			spritesLookup = new Dictionary<string, Sprite>();
			for (int i = 0; i < sprites.Length; i++)
			{
				if (!spritesLookup.ContainsKey(sprites[i].name))
				{
					spritesLookup.Add(sprites[i].name, sprites[i]);
				}
				else
				{
					Debug.LogWarning("Sprite with same name" + sprites[i].name + "is skipped from this container.");
				}
			}
		}
	}

	public Sprite GetResource(string name)
	{
		if (spritesLookup.ContainsKey(name))
		{
			return spritesLookup[name];
		}
		else
		{
			return null;
		}
	}

	public Sprite GetRandomResource()
	{
		if (sprites.Length > 0)
		{
			return sprites[Random.Range(0, sprites.Length)];
		}
		else
		{
			return null;
		}
	}
}
