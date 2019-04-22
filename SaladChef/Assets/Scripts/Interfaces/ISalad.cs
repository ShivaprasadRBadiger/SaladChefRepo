using System.Collections.Generic;

namespace SaladChef
{
	public interface ISalad : IItem
	{
		List<IVegetable> currentMix { get; set; }
	}
}