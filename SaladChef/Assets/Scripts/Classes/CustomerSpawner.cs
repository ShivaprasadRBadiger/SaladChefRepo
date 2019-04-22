using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace SaladChef
{
	public class CustomerSpawner : MonoBehaviour, ITickable
	{
		[SerializeField]
		private List<NpcCustomerBehaviour> npcCustomers;

		private float[] npcSlotIdleTimers;
		/// <summary>
		/// Defines minimum period an customer slot should remain empty after existing customer leave
		/// </summary>
		[SerializeField]
		private float minArrivalTime;
		/// <summary>
		/// Defines maximum period a customer slot can be empty.
		/// </summary>
		[SerializeField]
		private float maxArrivalTime;

		private void Awake()
		{
			npcSlotIdleTimers = new float[npcCustomers.Count];
			TickableManager.Instance.Subscribe(this);
			for (int i = 0; i < npcCustomers.Count; i++)
			{
				if (!npcCustomers[i].isActiveAndEnabled)
				{
					npcSlotIdleTimers[i] = GetRandomArrivalTime();
				}
			}
		}

		private float GetRandomArrivalTime()
		{
			return Random.Range(minArrivalTime, maxArrivalTime);
		}

		public void Tick()
		{
			for (int i = 0; i < npcCustomers.Count; i++)
			{
				if (!npcCustomers[i].isActiveAndEnabled)
				{
					npcSlotIdleTimers[i] -= Time.deltaTime;
					if (npcSlotIdleTimers[i] <= 0)
					{
						npcCustomers[i].gameObject.SetActive(true);
						npcSlotIdleTimers[i] = GetRandomArrivalTime();
					}
				}
			}
		}
	}
}
