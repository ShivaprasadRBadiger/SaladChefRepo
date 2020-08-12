namespace SaladChef
{
	public interface ISatisfactionChangedObserver : IObserver
	{
		void OnSatisfactionChanged(Satisfaction leavingNpcCustomer);
	}
}
