using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

namespace SaladChef
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class NpcCustomerBehaviour : MonoBehaviour, ITickable, INpcCustomer, ISubject
	{
		[SerializeField]
		SpriteContainer spriteContainer;
		[SerializeField]
		SpriteRenderer spriteRenderer;
		[SerializeField]
		private float angrySpeedMultiplier;
		[SerializeField]
		private CustomerHUD customerHUD;
		Dictionary<int, IVegetable> orderMenu;


		private float waitTimer;

		private float _waitingTime;
		public float waitingTime => _waitingTime;

		private bool _isAngry;
		public bool isAngry => _isAngry;

		private Satisfaction _satisfaction;
		public Satisfaction satisfaction => _satisfaction;

		public int id => this.GetInstanceID();

		private string _servicedBy = string.Empty;
		public string servicedBy => _servicedBy;

		private const string ERRMSG_WRONG_SUB = "Observer not supported!";
		private const int MIN_VEGGIE_IN_SALAD = 2;
		private const int MAX_VEGGIE_IN_SALAD = 3;
		Action<INpcCustomer> customerFeedbackListeners;
		VegetableFactory vegetableFactory;
		SaladFactory saladFactory;
		ISalad currentOrder;

		int[] vegetableKeys;

		private void Awake()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
			vegetableFactory = VegetableFactory.GetInstance();
			saladFactory = SaladFactory.GetInstance();
			orderMenu = vegetableFactory.GetCatalog();
			vegetableKeys = orderMenu.Keys.ToArray();
		}
		private void OnEnable()
		{
			spriteRenderer.sprite = spriteContainer.GetRandomResource();
			_satisfaction = Satisfaction.Excellent;
			_waitingTime = Random.Range(80, 140);
			waitTimer = _waitingTime;
			_isAngry = Random.value < 0.2f ? true : false;
			OrderUp();
			customerHUD.ShowProgress();
			TickableManager.Instance.Subscribe(this);
		}

		private void OrderUp()
		{
			ISalad orderSalad = saladFactory.GetSalad();
			for (int i = 0; i < Random.Range(MIN_VEGGIE_IN_SALAD, MAX_VEGGIE_IN_SALAD); i++)
			{
				orderSalad.currentMix.Add(GetRandomChopedVeggie());
			}
			currentOrder = orderSalad;
			customerHUD.UpdateOrderUI(orderSalad);
		}

		private IVegetable GetRandomChopedVeggie()
		{
			var veggie = (IVegetable)orderMenu[vegetableKeys[Random.Range(0, vegetableKeys.Length - 1)]].Clone();
			veggie.currentState |= ProcessingState.CHOPPED; //Customers can ask for boiled/fried/diced in future extensions.
			return veggie;
		}

		private void OnDisable()
		{
			float serviceQuality = (waitTimer / waitingTime);

			if (serviceQuality <= 0)
			{
				_satisfaction = Satisfaction.Bad;
			}
			else if (serviceQuality < 0.7f)
			{
				_satisfaction = Satisfaction.Good;
			}
			else
			{
				_satisfaction = Satisfaction.Excellent;
			}
			currentOrder = null;
			Notify();
		}

		#region ITickable Implementation
		public void Tick()
		{
			if (waitTimer > 0)
			{
				if (isAngry)
				{
					waitTimer -= Time.deltaTime;
				}
				else
				{
					waitTimer -= Time.deltaTime * angrySpeedMultiplier;
				}
				customerHUD.UpdateProgress(waitTimer / waitingTime);
			}
			else
			{
				customerHUD.HideProgress();
				TickableManager.Instance.Unsubscribe(this);
				gameObject.SetActive(false);
			}
		}
		#endregion

		#region ISubject Implementation
		public void Notify()
		{
			if (customerFeedbackListeners != null)
			{
				customerFeedbackListeners.Invoke(this);
			}
		}

		public void Register(IObserver observer)
		{
			var customerObserver = (ICustomerObserver)observer;
			if (customerObserver == null)
			{
				Debug.LogError(ERRMSG_WRONG_SUB);
			}
			customerFeedbackListeners += customerObserver.OnCustomerLeft;
		}

		public void Unregister(IObserver observer)
		{
			var customerObserver = (ICustomerObserver)observer;
			if (customerObserver == null)
			{
				Debug.LogError(ERRMSG_WRONG_SUB);
			}
			customerFeedbackListeners -= customerObserver.OnCustomerLeft;
		}
		/// <summary>
		/// Serves the order to customer , if right order is server customer accepts and leaves.
		/// else customer rejects the order.
		/// </summary>
		/// <param name="salad">Oder being served to this customer.</param>
		/// <returns>If Salad being served is same as the one that is ordered.</returns>
		public bool Service(ISalad salad, string chefID) //Can change this into abstract order to extend scope for other dishes.
		{
			if (currentOrder.IsSame(salad))
			{
				_servicedBy = chefID;
				customerHUD.HideProgress();
				TickableManager.Instance.Unsubscribe(this);
				gameObject.SetActive(false);
				Debug.Log("Hurray!CHOM CHOM CHOM CHOM....");
				return true;
			}
			else
			{
				Debug.Log("Wrong Order!:-( ...");
				return false;
			}
		}
		#endregion

	}
}

