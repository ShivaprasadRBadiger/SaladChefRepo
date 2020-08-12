using System.Collections;
using System.Collections.Generic;

namespace SaladChef
{
	internal interface IQueuInventory : IInventory
	{
		int capacity { get; }
		Queue<IItem> currentItems { get; }
		IItem GetItem();
		bool AddItem(IItem item);
		int RemoveItem();
		int Count();

	}
}