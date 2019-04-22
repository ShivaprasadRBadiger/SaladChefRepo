namespace SaladChef
{
	public abstract class InventoryFactoryBase
	{
		public abstract IInventory GetInventory(InventoryType type, int capacity);
	}
}
