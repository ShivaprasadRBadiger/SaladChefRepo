using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
	public class PlayerInputBindings : MonoBehaviour, IPlayerInputBindings
	{
		[Header("Input Bindings")]
		[SerializeField]
		private InputBindings inputBindings;

		public InputBindings GetInputBindings()
		{
			return inputBindings;
		}
	}
}

