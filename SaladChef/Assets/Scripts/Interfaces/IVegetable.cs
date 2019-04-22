using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
	public interface IVegetable : IItem, IProcessable, ICloneable
	{
		Dictionary<int, Sprite> stateSpriteDictionary { get; }
	}
}