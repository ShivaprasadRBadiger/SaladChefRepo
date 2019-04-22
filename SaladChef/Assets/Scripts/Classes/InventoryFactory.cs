using UnityEngine;
using System.Collections;

namespace SaladChef
{
	public class InventoryFactory : InventoryFactoryBase
	{
		private static InventoryFactory _instance;
		public static InventoryFactory GetInstance()
		{
			if (_instance == null)
			{
				_instance = new InventoryFactory();
			}

			return _instance;
		}
		private InventoryFactory() { }

		public override IInventory GetInventory(InventoryType type, int capacity)
		{
			switch (type)
			{
				case InventoryType.PlayerInventory: return new PlayerInventory(capacity);
				case InventoryType.Default: return new DefaultInventory(capacity);
				default: return new DefaultInventory(capacity);
			}
		}
	}
}

