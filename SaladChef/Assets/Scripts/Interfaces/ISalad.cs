using System.Collections.Generic;

namespace SaladChef
{
	public interface ISalad : IItem
	{
		List<IVegitable> currentMix { get; set; }
	}
}