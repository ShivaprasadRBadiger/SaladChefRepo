using System;

namespace SaladChef
{
	[Flags]
	[Serializable]
	//Sate of the vegetable, Ex: Raw,Chopped,Boiled,Steamed,Diced,Shredded,Fried etc.
	public enum ProcessingState
	{
		RAW = 1 << 0,
		CHOPPED = 1 << 1,
	}
}