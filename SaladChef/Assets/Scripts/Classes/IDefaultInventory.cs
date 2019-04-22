namespace SaladChef
{
	public interface IDefaultInventory : IInventory
	{
		bool AddItem(IItem item, int amount);
		int GetCurrentCapacity();
		bool HasItems(IItem item, int amount);
		bool RemoveItem(IItem item, int amount);
	}
}