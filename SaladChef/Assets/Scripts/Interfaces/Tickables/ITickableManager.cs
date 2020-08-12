namespace SaladChef
{
	public interface ITickableManager
	{
		void Subscribe(object tickTarget);
		void Unsubscribe(object tickTarget);
	}
}