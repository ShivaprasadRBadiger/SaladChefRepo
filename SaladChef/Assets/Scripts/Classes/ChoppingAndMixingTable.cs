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

		public string usedBy { get; set; }
		#endregion

		private void Awake()
		{
			TickableManager.Instance.Subscribe(this);
		}

		#region ITickable Implementation
		public void Tick()
		{
			if (isProcessing)
			{
				taskTimeElapsed += Time.deltaTime;
				progress = taskTimeElapsed / taskCompletionTime;
				if (progress >= 1)
				{
					HandleEndProcessing();
				}
				else
				{
					currentTaskHandler.OnTaskUpdate(progress);
				}
			}
		}
		#endregion

		private void HandleEndProcessing()
		{
			currentlyProcessing.currentState |= stateModifier;
			isProcessing = false;
			currentTaskHandler.OnTaksEnded();
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

		public bool MixSalad(IProcessable processable)
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
				return true;
			}
			else
			{
				Debug.LogError("Only chopped vegetables can be mixed into salad");
				return false;
			}
		}
	}
}

