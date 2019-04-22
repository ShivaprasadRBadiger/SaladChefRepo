using UnityEngine;

namespace SaladChef
{
	public interface IPlayerHUD
	{
		void RemoveCarryItem(int slotIndex);
		void SetCarryItem(Sprite itemSprite, int slotIndex);
		void SetPlayerTag(string playerTag);
	}
}