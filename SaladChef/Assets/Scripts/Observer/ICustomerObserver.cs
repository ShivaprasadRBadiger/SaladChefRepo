namespace SaladChef
{
	public interface ICustomerObserver : IObserver
	{
		void OnCustomerLeft(INpcCustomer leavingNpcCustomer);
	}
}
