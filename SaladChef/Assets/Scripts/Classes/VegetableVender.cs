using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SaladChef
{
	public class VegetableVender : MonoBehaviour, IVegetableVender
	{
		[SerializeField]
		SpriteRenderer[] spriteRenderers;
		/// <summary>
		/// Type of vegetable this vendor gives to player.
		/// </summary>
		[SerializeField]
		int vegetableID;

		VegetableFactory vegetableFactory;

		private void Awake()
		{
			vegetableFactory = VegetableFactory.GetInstance();
			var vegetableCatalog = vegetableFactory.GetCatalog();
			if (vegetableCatalog.ContainsKey(vegetableID))
			{
				SetVegitable(vegetableCatalog[vegetableID]);
			}
		}

		public void SetVegitable(IVegetable vegitable)
		{
			for (int i = 0; i < spriteRenderers.Length; i++)
			{
				spriteRenderers[i].sprite = vegitable.stateSpriteDictionary[(int)ProcessingState.RAW];
			}
		}

		public IVegetable VendVegetable()
		{
			return vegetableFactory.GetProduce(vegetableID);
		}

	}
}
