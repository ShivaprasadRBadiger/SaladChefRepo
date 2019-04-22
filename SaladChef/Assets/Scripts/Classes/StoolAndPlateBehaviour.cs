using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
	public class StoolAndPlateBehaviour : MonoBehaviour, IPlateBehaviour
	{
		[SerializeField]
		SpriteRenderer vegetableSpriteRenderer;

		IQueuInventory plateInventory;

		private void Awake()
		{
			plateInventory = InventoryFactory.GetInstance().GetInventory(InventoryType.QueueInventory, 1) as IQueuInventory;
		}

		public bool PlaceItem(IItem item)
		{
			bool result = plateInventory.AddItem(item);
			if (result)
			{
				UpdateSprite(item);
			}
			return result;
		}

		private void UpdateSprite(IItem item)
		{
			IVegetable vegetable = (IVegetable)item;
			if (vegetable != null)
			{
				this.vegetableSpriteRenderer.sprite = vegetable.stateSpriteDictionary[ProcessingState.RAW];
				this.vegetableSpriteRenderer.enabled = true;
			}
			else
			{
				this.vegetableSpriteRenderer.enabled = false;
			}
		}

		public IItem GetItem()
		{
			var item = plateInventory.GetItem();
			if (item != null)
			{
				plateInventory.RemoveItem();
				UpdateSprite(null);
				return item;
			}
			else
			{
				return null;
			}
		}
	}
}

