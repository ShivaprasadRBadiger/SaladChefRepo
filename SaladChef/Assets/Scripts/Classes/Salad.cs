using System.Collections.Generic;

namespace SaladChef
{
	internal class Salad : ISalad
	{
		public List<IVegetable> currentMix { get; set; }

		public Salad()
		{
			this.currentMix = new List<IVegetable>();
		}

		public int id { get { return 0; } }
		public string name { get { return string.Empty; } }

	}
}