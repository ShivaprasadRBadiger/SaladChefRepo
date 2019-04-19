using System;

namespace SaladChef
{
	public interface IProcessor
	{
		ProcessingState stateModifier { get; }
		void Process(IProcessable veggie, Action OnProccessed);
	}
}