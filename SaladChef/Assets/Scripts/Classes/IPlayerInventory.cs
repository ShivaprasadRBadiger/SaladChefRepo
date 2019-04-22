using System.Collections;
using System.Collections.Generic;

namespace SaladChef
{
	internal interface IPlayerInventory : IInventory
	{
		int capacity { get; }
		Queue<IItem> currentItems { get; }
		IItem GetItem();
		bool AddItem(IItem item);
		bool RemoveItem();
		int GetCurrentCapacity();

	}
}