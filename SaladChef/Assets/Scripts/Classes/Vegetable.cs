using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SaladChef
{
	public class Vegetable : IVegetable
	{
		public int id { get; }

		public string name { get; }

		public float processingTime { get; }

		public Dictionary<int, Sprite> stateSpriteDictionary { get; }

		public Vegetable(int id, string name, float processingTime, Dictionary<int, Sprite> stateSpriteDictionary)
		{
			this.id = id;
			this.name = name;
			this.processingTime = processingTime;
			this.initialState = ProcessingState.RAW;
			this.stateSpriteDictionary = stateSpriteDictionary;
		}
		public ProcessingState currentState { get; set; }

		private ProcessingState initialState { get; }


		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}
}

