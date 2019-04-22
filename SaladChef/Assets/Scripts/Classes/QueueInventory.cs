using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SaladChef
{
	public class QueueInventory : IQueuInventory
	{
		public int capacity { get; }
		public Queue<IItem> currentItems { get; }

		public QueueInventory(int capacity)
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

		public int Count()
		{
			return currentItems.Count;
		}

		public IItem GetItem()
		{
			if (currentItems.Count > 0)
			{
				return currentItems.Peek();
			}
			return null;
		}

		public int RemoveItem()
		{
			if (currentItems.Count > 0)
			{
				int index = currentItems.Count - 1;
				currentItems.Dequeue();
				return index;
			}
			else
			{
				return -1;
			}
		}
	}
}

