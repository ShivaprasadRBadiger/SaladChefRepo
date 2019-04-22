using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SaladChef
{
	internal class CustomerHUD : MonoBehaviour, ITimer
	{
		[SerializeField]
		Image waitingTimer;
		[SerializeField]
		GameObject requestingVeggiePrefab;
		[SerializeField]
		Gradient waitingColors;
		private Queue<GameObject> orderingItems = new Queue<GameObject>();

		public void HideProgress()
		{
			waitingTimer.transform.parent.gameObject.SetActive(false);
		}
		public void UpdateOrderUI(ISalad salad)
		{
			if (salad == null)
			{
				Debug.LogError("Salad to order is empty!");
				return;
			}
			ClearPreviouOrderIcons();
			for (int i = 0; i < salad.currentMix.Count; i++)
			{
				var veggieInOrder = Instantiate(requestingVeggiePrefab, requestingVeggiePrefab.transform.parent);
				veggieInOrder.GetComponent<Image>().sprite = salad.currentMix[i].stateSpriteDictionary[ProcessingState.RAW];
				orderingItems.Enqueue(veggieInOrder);
				veggieInOrder.SetActive(true);
			}
		}

		private void ClearPreviouOrderIcons()
		{
			while (orderingItems.Count > 0)
			{
				GameObject.Destroy(orderingItems.Dequeue());
			}
			orderingItems.Clear();
		}

		public void ShowProgress()
		{
			waitingTimer.transform.parent.gameObject.SetActive(true);
		}

		public void UpdateProgress(float progress)
		{
			waitingTimer.fillAmount = progress;
			waitingTimer.color = waitingColors.Evaluate(progress);
		}
	}
}