using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SaladChef
{
	[CreateAssetMenu(fileName = "VegitableData", menuName = "Items/VegitableData", order = 1)]
	[Serializable]
	public class VegetableData : ScriptableObject
	{
		public List<VegetableDatum> data;
	}

	[Serializable]
	public class VegetableDatum
	{
		public int id;
		public string name;
		public int processingTime;
		public VegitableView[] vegitableViews;
	}

	[Serializable]
	public class VegitableView
	{
		public int processingState;
		public Sprite sprite;
	}

}

