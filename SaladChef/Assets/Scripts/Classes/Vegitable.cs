using UnityEngine;
using System.Collections;

namespace SaladChef
{
	public class Vegitable : IVegitable
	{
		public int id { get; }

		public string name { get; }

		public float processingTime { get; }

		public Vegitable(int id, string name, float processingTime)
		{
			this.id = id;
			this.name = name;
			this.processingTime = processingTime;
			this.initialState = ProcessingState.RAW;
		}

		public ProcessingState currentState { get; set; }

		private ProcessingState initialState { get; }

		public void Reset()
		{
			currentState = initialState;
		}
	}
}

