using UnityEngine;

namespace SaladChef
{
	public interface IPlayerHUD : ITimer
	{
		void RemoveCarryItem();
		void SetCarryItem(Sprite itemSprite);
		void SetPlayerTag(string playerTag);
		void CarrySalad();
		void DropSalad();
		void Setup(PlayerData playerData);
	}
}