using UnityEngine;
using System.Collections;
using System;

namespace SaladChef
{

	public class ChoppingAndMixingTable : MonoBehaviour, IProcessor, ITickable, IChoppingAndMixingTable
	{
		[SerializeField]
		SpriteRenderer saladSprite;

		private float progress = 0;
		private float taskCompletionTime = 0;
		private float taskTimeElapsed = 0;
		private IProcessable currentlyProcessing;
		private ITaskHandler currentTaskHandler;

		#region IProcessor Implementation
		public bool isProcessing { get; set; }
		ISalad currentSalad
		{
			get;
			set;
		}
		public ProcessingState stateModifier
		{
			get { return ProcessingState.CHOPPED; }
		}

		public string usedBy { get; set; }
		#endregion

		private void Awake()
		{
			TickableManager.Instance.Subscribe(this);
		}





		public void Process(IProcessable veggie, ITaskHandler taskHandler)
		{
			if (!isProcessing)
			{
				currentlyProcessing = veggie;
				isProcessing = true;
				taskCompletionTime = veggie.processingTime;
				taskTimeElapsed = 0;
				progress = 0;
				currentTaskHandler = taskHandler;
				taskHandler.OnStartedTask();
			}
			else
			{
				Debug.LogError("This processor is busy processing.");
			}
		}

		public void Tick()
		{
			if (isProcessing)
			{
				taskTimeElapsed += Time.deltaTime;
				progress = taskTimeElapsed / taskCompletionTime;
				if (progress >= 1)
				{
					currentlyProcessing.currentState |= stateModifier;
					isProcessing = false;
					currentTaskHandler.OnTaksEnded();
					HandleEneProcessing();
				}
				else
				{
					currentTaskHandler.OnTaskUpdate(progress);
				}
			}
		}

		private void HandleEneProcessing()
		{
			MixSalad(currentlyProcessing);
			currentlyProcessing = null;
		}

		public ISalad PickupSalad()
		{
			var finalSalad = currentSalad;
			currentSalad = null;
			saladSprite.enabled = false;
			return finalSalad;
		}

		public void MixSalad(IProcessable processable)
		{
			var vegetable = (IVegetable)processable;
			if (vegetable != null && vegetable.currentState.HasFlag(ProcessingState.CHOPPED))
			{
				if (currentSalad == null)
				{
					saladSprite.enabled = true;
					currentSalad = SaladFactory.GetInstance().GetSalad();
				}
				currentSalad.currentMix.Add(vegetable);

			}
			else
			{
				Debug.LogError("Only chopped vegetables can be mixed into salad");
			}
		}
	}
}

