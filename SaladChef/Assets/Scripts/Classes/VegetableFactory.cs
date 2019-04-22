using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SaladChef
{
	/// <summary>
	/// Creates requested vegetables.
	/// </summary>
	public class VegetableFactory : VegetableFactoryBase
	{
		private static VegetableFactory _instance;
		public static VegetableFactory GetInstance()
		{
			if (_instance == null)
			{
				_instance = new VegetableFactory();
			}

			return _instance;
		}

		readonly Dictionary<int, IVegetable> vegitables = new Dictionary<int, IVegetable>();

		public Dictionary<int, IVegetable> GetCatalog()
		{
			return vegitables;
		}

		private VegetableFactory()
		{
			List<VegetableDatum> vegetableCatalog = Resources.Load<VegetableData>("VegitableData").data;

			for (int i = 0; i < vegetableCatalog.Count; i++)
			{
				Dictionary<ProcessingState, Sprite> stateSpriteDictionary = new Dictionary<ProcessingState, Sprite>();
				for (int j = 0; j < vegetableCatalog[i].vegitableViews.Length; j++)
				{
					stateSpriteDictionary.Add(vegetableCatalog[i].vegitableViews[j].processingState, vegetableCatalog[i].vegitableViews[j].sprite);
				}
				vegitables.Add(vegetableCatalog[i].id, new Vegetable(vegetableCatalog[i].id, vegetableCatalog[i].name, vegetableCatalog[i].processingTime, stateSpriteDictionary));
			}
		}

		public override IVegetable GetProduce(int id)
		{
			if (vegitables.ContainsKey(id))
			{
				return (Vegetable)vegitables[id].Clone();
			}
			else
			{
				Debug.LogError("Vegetable id not found!");
				return null as Vegetable;
			}
		}
	}
}

