using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SaladChef
{
	public class PlayerHUD : MonoBehaviour, IPlayerHUD
	{
		/// <summary>
		/// Player's Identification tag.
		/// </summary>
		[SerializeField]
		Text playerTag;
		/// <summary>
		/// Visual indication of player's current timed task progress.
		/// </summary>
		[SerializeField]
		Image progress;
		/// <summary>
		/// Visual indication of player carrying salad
		/// </summary>
		[SerializeField]
		Image doubleHandCarry;

		/// <summary>
		/// Visual indications of what items are carried by this player.
		/// </summary>
		[SerializeField]
		GameObject carryItemPrefab;

		private Queue<GameObject> carryingItems = new Queue<GameObject>();

		public void SetPlayerTag(string playerTag)
		{
			this.playerTag.text = playerTag;
		}
		public void SetCarryItem(Sprite itemSprite)
		{
			if (itemSprite)
			{
				var newCarryItem = Instantiate(carryItemPrefab, carryItemPrefab.transform.parent);
				newCarryItem.GetComponent<Image>().sprite = itemSprite;
				carryingItems.Enqueue(newCarryItem);
				newCarryItem.SetActive(true);
			}
		}
		public void RemoveCarryItem()
		{
			if (carryingItems.Count > 0)
			{
				GameObject.Destroy(carryingItems.Dequeue());
			}
		}

		public void ShowProgress()
		{
			progress.transform.parent.gameObject.SetActive(true);//Can be done better
		}

		public void UpdateProgress(float progress)
		{
			this.progress.fillAmount = progress;
		}

		public void HideProgress()
		{
			progress.transform.parent.gameObject.SetActive(false);//Can be done better
		}

		public void CarrySalad()
		{
			doubleHandCarry.enabled = true;
		}
		public void DropSalad()
		{
			doubleHandCarry.enabled = false;
		}

		public void Setup(PlayerData playerData)
		{
			SetPlayerTag(playerData.playerName);
			SetPlayerTagColor(playerData.playerColor);
			SetProgressBarColor(playerData.playerColor);
		}

		private void SetProgressBarColor(Color playerColor)
		{
			progress.color = playerColor;
		}

		private void SetPlayerTagColor(Color playerColor)
		{
			playerTag.color = playerColor;
		}
	}
}