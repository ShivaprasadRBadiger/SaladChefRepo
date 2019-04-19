using System;

namespace SaladChef
{
	[Flags]
	public enum ProcessingState
	{
		RAW = 0,
		CHOPPED = 1 << 0,
	}
}