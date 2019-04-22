namespace SaladChef
{
	public interface IDefaultInventory : IInventory
	{
		bool AddItem(IItem item, int amount);
		bool HasCapacityReached();
		bool HasItems(IItem item = null, int amount = 1);
		bool RemoveItem(IItem item, int amount);
	}
}