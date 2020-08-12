using System;

namespace SaladChef
{
	[Flags]
	[Serializable]
	//Sate of the vegetable, Ex: Raw,Chopped,Boiled,Steamed,Diced,Shredded,Fried etc.
	public enum ProcessingState
	{
		RAW = 1 << 0, // = 1    00001
		CHOPPED = 1 << 1, //= 2 00010
		FRIED = 1 << 2, // = 4    00100
	}
}