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

		/// <summary>
		///Compares two salads,they are same if same number type and state of each vegetable in salad are same.
		/// </summary>
		/// <param name="salad">Salad that has to be compared with this.</param>
		/// <returns>If salad are same returns true else false.</returns>
		public bool IsSame(ISalad salad)
		{
			if (currentMix.Count != salad.currentMix.Count)
				return false;

			for (int i = 0; i < salad.currentMix.Count; i++)
			{
				if (currentMix[i].id != salad.currentMix[i].id)
				{
					return false;

				}
				else if (currentMix[i].currentState != salad.currentMix[i].currentState)
				{
					return false;
				}
			}
			return true;
		}
	}
}