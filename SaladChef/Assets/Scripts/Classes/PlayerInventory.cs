using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SaladChef
{
	public class PlayerInventory : IPlayerInventory
	{
		public int capacity { get; }
		public Queue<IItem> currentItems { get; }

		public PlayerInventory(int capacity)
		{
			this.capacity = capacity;
			currentItems = new Queue<IItem>(capacity);
		}
		public bool AddItem(IItem item)
		{
			#region Validation
			if (item == null)
			{
				Debug.LogError("Argument cannot be null.");
				return false;
			}
			if (currentItems.Count >= capacity)
			{
				Debug.LogWarning("Player cant carry any more.");
				return false;
			}
			#endregion

			currentItems.Enqueue(item);
			return true;
		}

		public int GetCurrentCapacity()
		{
			return currentItems.Count;
		}

		public IItem GetItem()
		{
			return currentItems.Peek();
		}

		public bool RemoveItem()
		{
			if (currentItems.Count > 0)
			{
				return currentItems.Dequeue() != null;
			}
			else
			{
				return false;
			}
		}
	}
}

