using System;

namespace SaladChef
{
	/// <summary>
	/// Can be extended to Ovens, Blenders ,Grinders,Refrigerator etc...
	/// </summary>
	public interface IProcessor
	{
		/// <summary>
		/// Used to block access to other players.
		/// </summary>
		string usedBy { get; set; }
		/// <summary>
		/// If some one is actively using this processor.
		/// </summary>
		bool isProcessing { set; get; }
		/// <summary>
		/// This state is added to existing state when processing is done by the device.
		/// </summary>
		ProcessingState stateModifier { get; }
		/// <summary>
		/// This is async process that completes processing the processable in passed amount of time in processable.
		/// </summary>
		/// <param name="processable">A IProcessable that should be processed by this device.</param>
		/// <param name="taskHandelr">ITaskHandler keep track of task status through start till end.</param>
		void Process(IProcessable processable, ITaskHandler taskHandelr);
	}
}