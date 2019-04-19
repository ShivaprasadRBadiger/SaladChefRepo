using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SaladChef
{

	/// <summary>
	/// Manages life cycle of all <see cref="tickables"/> ,<see cref="fixedTickables"/> and <see cref="lateTickables"/> in game.
	/// </summary>
	public class GameManager : MonoBehaviour, ITickableManager
	{
		#region Singleton Implementation
		public static GameManager _Instance;
		public static GameManager Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new GameObject("GameManager", typeof(GameManager)).GetComponent<GameManager>();
					DontDestroyOnLoad(_Instance);
				}
				return _Instance;
			}
		}
		#endregion

		#region Private properties
		private Action tickables;
		private Action fixedTickables;
		private Action lateTickables;
		#endregion

		#region ITickableManager Implementation

		public void Subscribe(object tickTarget)
		{
			ITickable tickable = tickTarget as ITickable;
			if (tickable != null)
			{
				tickables += tickable.Tick;
			}
			IFixedTickable fixedTickable = tickTarget as IFixedTickable;
			if (fixedTickable != null)
			{
				fixedTickables += fixedTickable.FixedTick;
			}
			ILateTickable lateTickable = tickTarget as ILateTickable;
			if (lateTickable != null)
			{
				lateTickables += lateTickable.LateTick;
			}
		}

		public void Unsubscribe(object tickTarget)
		{
			ITickable tickable = tickTarget as ITickable;
			if (tickable != null)
			{
				tickables -= tickable.Tick;
			}
			IFixedTickable fixedTickable = tickTarget as IFixedTickable;
			if (fixedTickable != null)
			{
				fixedTickables -= fixedTickable.FixedTick;
			}
			ILateTickable lateTickable = tickTarget as ILateTickable;
			if (lateTickable != null)
			{
				lateTickables -= lateTickable.LateTick;
			}
		}

		#endregion

		#region MonoBehaviour Callbacks
		void Update()
		{
			if (tickables != null)
			{
				tickables.Invoke();
			}
		}

		void FixedUpdate()
		{
			if (fixedTickables != null)
			{
				fixedTickables.Invoke();
			}
		}

		void LateUpdate()
		{
			if (lateTickables != null)
			{
				lateTickables.Invoke();
			}
		}
		#endregion

		#region Public Methods
		public void InstanceReset()
		{
			tickables = null;
			fixedTickables = null;
			lateTickables = null;
		}
		#endregion

	}
}
