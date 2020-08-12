namespace SaladChef
{
	public interface IChoppingAndMixingTable
	{
		ISalad PickupSalad();
		bool MixSalad(IProcessable vegetable);
	}
}