using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SaladChef
{
	/// <summary>
	/// Class is holds all IItem types and can only hold "capacity" number of IItem.
	/// </summary>
	public class DefaultInventory : IDefaultInventory
	{
		private Dictionary<IItem, int> dictionary;
		private int capacity;

		public DefaultInventory(int capacity)
		{
			dictionary = new Dictionary<IItem, int>();
		}
		/// <summary>
		/// Adds a IItem type to inventory , return true if inventory can hold it else returns false.
		/// </summary>
		/// <param name="item">Item type to be stored</param>
		/// <param name="amount">Number of items this inventory can hold.</param>
		/// <returns>True if requested amount to added does not exceed capacity else false</returns>
		public bool AddItem(IItem item, int amount)
		{
			if (amount <= (capacity + amount))
			{
				if (dictionary.ContainsKey(item))
				{
					dictionary[item] += amount;
				}
				else
				{
					dictionary.Add(item, amount);
				}
				capacity += amount;
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Removes specified item and number or items.
		/// </summary>
		/// <param name="item">Kind of item to be removed.</param>
		/// <param name="amount">Number of items of above kind should be removed</param>
		/// <returns>Returns true if item of given number can removed and removes them else returns false and wont remove any amount.</returns>
		public bool RemoveItem(IItem item, int amount)
		{
			if (dictionary.ContainsKey(item))
			{
				if (dictionary[item] > amount)
				{
					dictionary[item] -= amount;
				}
				else if (dictionary[item] == amount)
				{
					dictionary.Remove(item);
				}
				else
				{
					return false;
				}
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Method returns true if requested type and amount of items are available in inventory.
		/// </summary>
		/// <param name="item">Type of item to check.</param>
		/// <param name="amount">Amount of item to check for.</param>
		/// <returns>True if requested amount and kind are available.</returns>
		public bool HasItems(IItem item, int amount)
		{
			if (dictionary.ContainsKey(item))
			{
				return dictionary[item] >= amount;
			}
			else
			{
				return false;
			}
		}

		public int GetCurrentCapacity()
		{
			if (dictionary != null)
			{
				return dictionary.Count;
			}
			else
			{
				return 0;
			}
		}

	}
}

