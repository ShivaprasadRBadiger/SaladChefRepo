using UnityEngine;

namespace SaladChef
{
	public interface IPlayerHUD
	{
		void RemoveCarryItem();
		void SetCarryItem(Sprite itemSprite);
		void SetPlayerTag(string playerTag);
		void ShowProgress();
		void UpdateProgress(float progress);
		void HideProgress();
		void CarrySalad();
		void DropSalad();
		void Setup(PlayerData playerData);
	}
}