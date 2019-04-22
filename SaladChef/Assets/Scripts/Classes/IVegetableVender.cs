namespace SaladChef
{
	public interface IVegetableVender
	{
		void SetVegitable(IVegetable vegitable);
		IVegetable VendVegetable();
	}
}