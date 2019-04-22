namespace SaladChef
{
	public interface IChoppingAndMixingTable
	{
		ISalad PickupSalad();
		void MixSalad(IProcessable vegetable);
	}
}