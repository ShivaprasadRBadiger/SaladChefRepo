namespace SaladChef
{
	public interface ITimer
	{
		void HideProgress();
		void ShowProgress();
		void UpdateProgress(float progress);
	}
}