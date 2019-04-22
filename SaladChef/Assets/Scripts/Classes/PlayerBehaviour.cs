using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SaladChef
{
	[RequireComponent(typeof(IInputBindings))]
	[RequireComponent(typeof(IPlayerMovement))]
	public class PlayerBehaviour : MonoBehaviour, ITickable
	{
		private IPlayerHUD playerHUD;
		private InputBindings inputBindings;
		private IPlayerInventory playerInventory;
		private IVegetableVender currentVender;
		private IProcessor vegetableChopperMixer;
		private IPlayerMovement playerMovement;

		private void Awake()
		{
			playerHUD = gameObject.GetComponentInChildren<IPlayerHUD>();
			playerHUD.SetPlayerTag(gameObject.tag);
			playerMovement = GetComponent<IPlayerMovement>();
			inputBindings = GetComponent<PlayerInputBindings>().GetInputBindings();
			playerInventory = (IPlayerInventory)InventoryFactory.GetInstance().GetInventory(InventoryType.PlayerInventory, 2);
		}


		public void Tick()
		{
			if (Input.GetKeyUp(inputBindings.Action1))
			{
				HandleAction1();
			}
			if (Input.GetKeyUp(inputBindings.Action2))
			{
				HandleAction2();
			}
		}

		private void HandleAction2()
		{
			if (vegetableChopperMixer != null)
			{
				IProcessable processable = (IProcessable)playerInventory.GetItem();
				if (processable != null)
				{
					playerMovement.DisableControls();
					vegetableChopperMixer.Process(processable, OnProcessed);
				}
				else
				{
					Debug.LogWarning("Nothing to chop");//Warn player with GUI
				}
			}
		}

		private void OnProcessed()
		{
			playerMovement.EnableControls();
		}

		private void HandleAction1()
		{
			if (currentVender != null)
			{
				var pickedupVegetable = currentVender.VendVegetable();
				if (pickedupVegetable != null)
				{
					if (playerInventory.AddItem(pickedupVegetable))
					{
						playerHUD.SetCarryItem(pickedupVegetable.stateSpriteDictionary[(int)pickedupVegetable.currentState], playerInventory.GetCurrentCapacity());
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
				vegetableChopperMixer = processor;
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
			if (processor == vegetableChopperMixer)
			{
				vegetableChopperMixer = null;
			}
		}
	}
}

