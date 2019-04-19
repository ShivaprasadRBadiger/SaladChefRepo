using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SaladChef
{
	[CreateAssetMenu(fileName = "VegitableData", menuName = "Items/VegitableData", order = 1)]
	[Serializable]
	public class VegitableData : ScriptableObject
	{
		public List<VegitableDatum> data;
	}

	[Serializable]
	public class VegitableDatum
	{
		public int id;
		public string name;
		public int processingTime;
		public Sprite rawSprite;
		public Sprite choppedSprite;
	}
}

