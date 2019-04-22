namespace SaladChef
{
	public interface IPlateBehaviour
	{
		IItem GetItem();
		bool PlaceItem(IItem item);
	}
}