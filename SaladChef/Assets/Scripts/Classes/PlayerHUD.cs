using UnityEngine;
using UnityEngine.UI;

namespace SaladChef
{
	public class PlayerHUD : MonoBehaviour, IPlayerHUD
	{
		[SerializeField]
		Text playerTag;
		[SerializeField]
		Image[] carryItems;

		public void SetPlayerTag(string playerTag)
		{
			this.playerTag.text = playerTag;
		}
		public void SetCarryItem(Sprite itemSprite, int slotIndex)
		{
			if (itemSprite && slotIndex >= 0)
			{
				carryItems[slotIndex].color = Color.white;
				carryItems[slotIndex].sprite = itemSprite;
			}
		}
		public void RemoveCarryItem(int slotIndex)
		{
			if (slotIndex >= 0)
			{
				carryItems[slotIndex].sprite = null;
				carryItems[slotIndex].color = Color.clear;
			}
		}
	}
}