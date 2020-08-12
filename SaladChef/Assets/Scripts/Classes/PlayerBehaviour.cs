using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SaladChef
{
	[RequireComponent(typeof(IInputBindings))]
	[RequireComponent(typeof(IPlayerMovement))]
	public class PlayerBehaviour : MonoBehaviour, ITickable, ITaskHandler
	{
		[SerializeField]
		PlayerData playerData;
		private IPlayerHUD playerHUD;
		private InputBindings inputBindings;
		/// <summary>
		/// Player's carry capacity and management.
		/// </summary>
		private IQueuInventory playerInventory;
		/// <summary>
		/// Current vegetable pile player is in range to use.
		/// </summary>
		private IVegetableVender currentVender;
		/// <summary>
		/// Any processor the player is in range to use.
		/// </summary>
		private IProcessor currentProcessor;
		/// <summary>
		/// Trash can the player is in range to use.
		/// </summary>
		private ITrashCan currentTrashCan;
		private IPlateBehaviour currentPlate;
		private IPlayerMovement playerMovement;
		private PlayerTask currentTask = PlayerTask.None;
		/// <summary>
		/// Current customer to whom this player is in range to serve.
		/// </summary>
		private INpcCustomer currentCustomer;

		private void Awake()
		{
			TickableManager.Instance.Subscribe(this);
			playerHUD = gameObject.GetComponentInChildren<IPlayerHUD>();
			playerHUD.Setup(playerData);
			playerMovement = GetComponent<IPlayerMovement>();
			inputBindings = GetComponent<PlayerInputBindings>().GetInputBindings();
			playerInventory = (IQueuInventory)InventoryFactory.GetInstance().GetInventory(InventoryType.QueueInventory, 2);
		}


		public void Tick()
		{
			if (Input.GetKeyUp(inputBindings.Action1))
			{
				HandleAction1();
			}
		}


		private void HandleAction1()
		{
			if (currentTask == PlayerTask.None)
			{
				PickupVegetable();
				GetOrPutOnPlate();
				ChopVegetable();
				PickupSalad();
			}
			if (currentTask == PlayerTask.Delivery)
			{
				DisposeSalad();
				Serve();
			}
		}

		private void PickupSalad()
		{
			#region Checks
			if (currentProcessor == null)
			{
				return;
			}
			var choppingAndMixingTable = (IChoppingAndMixingTable)currentProcessor;
			if (choppingAndMixingTable == null)
			{
				return;
			}
			if (playerInventory.Count() != 0)
			{
				return;
			}
			if (currentTask != PlayerTask.None)
			{
				return;
			}
			if (string.IsNullOrEmpty(currentProcessor.usedBy) && !gameObject.CompareTag(currentProcessor.usedBy))
			{
				return;
			}
			#endregion

			var salad = choppingAndMixingTable.PickupSalad();
			playerInventory.AddItem(salad);
			playerHUD.CarrySalad();
			currentTask = PlayerTask.Delivery;
			currentProcessor.usedBy = string.Empty;
		}
		private void Serve()
		{
			#region Checks
			if (currentCustomer == null)
			{
				return;
			}
			if (currentTask != PlayerTask.Delivery)
			{
				return;
			}
			var salad = (ISalad)playerInventory.GetItem();
			if (salad == null)
			{
				Debug.Log("Cannot server anything other than salad to customers right now...");
				return;
			}
			#endregion

			if (currentCustomer.Service(salad, gameObject.tag))
			{
				playerHUD.DropSalad();
				playerInventory.RemoveItem();
				currentTask = PlayerTask.None;
			}
		}
		private void DisposeSalad()
		{
			if (currentTrashCan == null)
			{
				return;
			}
			if (currentTask != PlayerTask.Delivery)
			{
				return;
			}
			playerHUD.DropSalad();
			playerInventory.RemoveItem();
			currentTask = PlayerTask.None;
		}
		private void GetOrPutOnPlate()
		{
			#region Checks
			if (currentPlate == null)
			{
				return;
			}
			if (currentTask != PlayerTask.None)
			{
				return;
			}
			#endregion
			if (!GetVegetableFromPlate())
			{
				if (playerInventory.Count() > 0)
				{
					PutVegetableOnPlate();
				}
			}
			else
			{
				Debug.Log("Got Vegetable out of plate");
			}

		}
		private bool GetVegetableFromPlate()
		{
			if (playerInventory.Count() >= playerInventory.capacity)
			{
				return false;
			}
			var veggie = (IVegetable)currentPlate.GetItem();
			if (veggie == null)
			{
				Debug.Log("Plate did not have vegetable.");
				return false;
			}

			playerInventory.AddItem(veggie);
			playerHUD.SetCarryItem(veggie.stateSpriteDictionary[ProcessingState.RAW]);
			return true;
		}
		private bool PutVegetableOnPlate()
		{
			if (currentPlate.PlaceItem(playerInventory.GetItem()))
			{
				playerInventory.RemoveItem();
				playerHUD.RemoveCarryItem();
				return true;
			}
			else
			{
				Debug.Log("Plate is full");
				return false;
			}
		}
		private void OnProcessed()
		{
			playerMovement.EnableControls();
		}
		private void ChopVegetable()
		{
			#region Checks
			if (currentProcessor == null)
			{
				return;
			}
			if (currentProcessor.isProcessing)
			{
				Debug.Log("This processor is already in use.");
				return;
			}
			if (playerInventory.Count() <= 0)
			{
				return;
			}
			var vegetable = (IVegetable)playerInventory.GetItem();
			if (vegetable == null)
			{
				Debug.LogError("Item in inventory is no a vegetable!");
				return;
			}
			if (!string.IsNullOrEmpty(currentProcessor.usedBy) && !gameObject.CompareTag(currentProcessor.usedBy))
			{
				return;
			}
			#endregion

			currentTask = PlayerTask.Chopping;
			currentProcessor.usedBy = gameObject.tag;
			currentProcessor.Process(vegetable, this);
			playerHUD.RemoveCarryItem();
			playerInventory.RemoveItem();
		}
		private void PickupVegetable()
		{
			if (currentVender != null)
			{
				var pickedupVegetable = currentVender.VendVegetable();
				if (pickedupVegetable != null)
				{
					if (playerInventory.AddItem(pickedupVegetable))
					{
						playerHUD.SetCarryItem(pickedupVegetable.stateSpriteDictionary[ProcessingState.RAW]);
					}
				}
			}
		}


		private void OnTriggerEnter2D(Collider2D other)
		{
			IVegetableVender veggieVender = other.GetComponent<IVegetableVender>();
			if (veggieVender != null)
			{
				currentVender = veggieVender;
			}
			IProcessor processor = other.GetComponent<IProcessor>();
			if (processor != null)
			{
				currentProcessor = processor;
			}
			ITrashCan trashCan = other.GetComponent<ITrashCan>();
			if (trashCan != null)
			{
				currentTrashCan = trashCan;
			}
			IPlateBehaviour plate = other.GetComponent<IPlateBehaviour>();
			if (plate != null)
			{
				currentPlate = plate;
			}
			INpcCustomer npcCustomer = other.GetComponent<INpcCustomer>();
			if (npcCustomer != null)
			{
				currentCustomer = npcCustomer;
			}
		}
		private void OnTriggerExit2D(Collider2D other)
		{
			IVegetableVender veggieVender = other.GetComponent<IVegetableVender>();
			if (veggieVender == currentVender)
			{
				currentVender = null;
			}
			IProcessor processor = other.GetComponent<IProcessor>();
			if (processor == currentProcessor)
			{
				currentProcessor = null;
			}
			ITrashCan trashCan = other.GetComponent<ITrashCan>();
			if (currentTrashCan == trashCan)
			{
				currentTrashCan = null;
			}
			IPlateBehaviour plate = other.GetComponent<IPlateBehaviour>();
			if (currentPlate == plate)
			{
				currentPlate = null;
			}
			INpcCustomer npcCustomer = other.GetComponent<INpcCustomer>();
			if (currentCustomer == npcCustomer)
			{
				currentCustomer = null;
			}
		}

		public void OnStartedTask()
		{
			switch (currentTask)
			{
				case PlayerTask.Chopping:
					playerHUD.ShowProgress();
					playerMovement.DisableControls();
					break;
			}
		}
		public void OnTaskUpdate(float progress)
		{
			switch (currentTask)
			{
				case PlayerTask.Chopping:
					playerHUD.UpdateProgress(progress);
					break;
			}
		}
		public void OnTaksEnded()
		{
			switch (currentTask)
			{
				case PlayerTask.Chopping:
					playerHUD.HideProgress();
					playerMovement.EnableControls();
					currentTask = PlayerTask.None;
					break;
			}
		}
	}
}

