using System;

namespace SaladChef
{
	[Flags]
	[Serializable]
	public enum ProcessingState
	{
		RAW = 0,
		CHOPPED = 1 << 0,
	}
}