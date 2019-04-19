using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SaladChef
{
	/// <summary>
	/// Creates requested vegetables.
	/// </summary>
	public class VegitableFactory
	{
		public static IVegitable[] GetAllVegitables()
		{
			List<VegitableDatum> veggieData = Resources.Load<VegitableData>("VegitableData").data;
			IVegitable[] vegitables = new IVegitable[veggieData.Count];
			for (int i = 0; i < veggieData.Count; i++)
			{
				vegitables[i] = new Vegitable(veggieData[i].id, veggieData[i].name, veggieData[i].processingTime);
			}
			return vegitables;
		}
	}
}

