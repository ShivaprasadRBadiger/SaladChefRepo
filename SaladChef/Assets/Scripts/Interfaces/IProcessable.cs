namespace SaladChef
{
	/// <summary>
	/// Anything that can be processed.(ex. Meat,Fruits,Veggies etc..)
	/// </summary>
	public interface IProcessable
	{
		/// <summary>
		/// Amount of time it takes to call OnProcessed on the processor.
		/// </summary>
		float processingTime { get; }
		ProcessingState currentState { set; get; }
	}
}