namespace SaladChef
{
	public interface ITaskHandler
	{
		void OnStartedTask();
		void OnTaksEnded();
		void OnTaskUpdate(float progress);
	}
}