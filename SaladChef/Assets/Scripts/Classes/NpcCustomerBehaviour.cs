using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

namespace SaladChef
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class NpcCustomerBehaviour : MonoBehaviour, ITickable, INpcCustomer, IObservable
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

		private List<string> _servicedBy = new List<string>();
		public List<string> servicedBy => _servicedBy;

		private int _orderNumber = 1;
		public int orderNumber => _orderNumber;

		private const string ERRMSG_WRONG_OBSERVER = "Observer not supported!";
		private const int MIN_VEGGIE_IN_SALAD = 2;
		private const int MAX_VEGGIE_IN_SALAD = 3;

		Action<INpcCustomer> customerFeedbackListeners;
		Action<Satisfaction> customerSatisfactionChangedListeners;

		VegetableFactory vegetableFactory;
		SaladFactory saladFactory;
		ISalad currentOrder;

		const float SIMPLE_ORDER_WAIT_TIME = 60;
		const float COMPLEX_ORDER_WAIT_TIME = 100;

		private int[] vegetableKeys;

		private float _serviceQuality;
		private float serviceQuality
		{
			get
			{
				return _serviceQuality;
			}
			set
			{
				if (_serviceQuality >= (int)satisfaction && value < (int)satisfaction)
				{
					customerSatisfactionChangedListeners?.Invoke(_satisfaction);
					Debug.Log("Satisfaction changed to " + _satisfaction.ToString());
					_satisfaction = (Satisfaction)((int)satisfaction - 25);
				}
				_serviceQuality = value;
			}

		}

		private void Awake()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
			vegetableFactory = VegetableFactory.GetInstance();
			saladFactory = SaladFactory.GetInstance();
			orderMenu = vegetableFactory.GetCatalog();
			vegetableKeys = orderMenu.Keys.ToArray();
		}

		public void Initialize(int orderNumber)
		{
			this._orderNumber = orderNumber;
			_satisfaction = Satisfaction.Excellent;
			_servicedBy.Clear();
			_isAngry = false;
		}

		private void OnEnable()
		{
			OrderUp();
			customerHUD.ShowProgress();
			spriteRenderer.sprite = spriteContainer.GetRandomResource();
			TickableManager.Instance.Subscribe(this);
		}

		private void OrderUp()
		{
			ISalad orderSalad = saladFactory.GetSalad();
			for (int i = 0; i < Random.Range(MIN_VEGGIE_IN_SALAD, MAX_VEGGIE_IN_SALAD + 1); i++)
			{
				orderSalad.currentMix.Add(GetRandomChopedVeggie());
			}
			currentOrder = orderSalad;
			DecideWaitingTime();
			customerHUD.UpdateOrderUI(orderSalad);
		}

		private void DecideWaitingTime()
		{
			if (currentOrder.currentMix.Count == 2)
			{
				_waitingTime = SIMPLE_ORDER_WAIT_TIME;
				waitTimer = _waitingTime;
			}
			else if (currentOrder.currentMix.Count > 2)
			{
				_waitingTime = COMPLEX_ORDER_WAIT_TIME;
				waitTimer = _waitingTime;
			}
		}

		private IVegetable GetRandomChopedVeggie()
		{
			var veggie = (IVegetable)orderMenu[vegetableKeys[Random.Range(0, vegetableKeys.Length)]].Clone();
			veggie.currentState |= ProcessingState.CHOPPED; //Customers can ask for boiled/fried/diced in future extensions.
			return veggie;
		}

		private void OnDisable()
		{
			serviceQuality = (waitTimer / waitingTime) * 100;

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

		#region IObservable Implementation
		public void Notify()
		{
			if (customerFeedbackListeners != null)
			{
				customerFeedbackListeners.Invoke(this);
			}
		}

		public void Register(IObserver observer)
		{
			if (observer is ICustomerObserver)
			{
				var customerObserver = (ICustomerObserver)observer;
				customerFeedbackListeners += customerObserver.OnCustomerLeft;
			}
			if (observer is ISatisfactionChangedObserver)
			{
				var satisfactionChangedObserver = (ISatisfactionChangedObserver)observer;
				customerSatisfactionChangedListeners += satisfactionChangedObserver.OnSatisfactionChanged;
			}
		}

		public void Unregister(IObserver observer)
		{
			if (observer is ICustomerObserver)
			{
				var customerObserver = observer as ICustomerObserver;
				customerFeedbackListeners -= customerObserver.OnCustomerLeft;
			}
			if (observer is ISatisfactionChangedObserver)
			{
				var satisfactionChangedObserver = observer as ISatisfactionChangedObserver;
				customerSatisfactionChangedListeners -= satisfactionChangedObserver.OnSatisfactionChanged;
			}
		}
		/// <summary>
		/// Serves the order to customer , if right order is server customer accepts and leaves.
		/// else customer rejects the order.
		/// </summary>
		/// <param name="salad">Oder being served to this customer.</param>
		/// <returns>If Salad being served is same as the one that is ordered.</returns>
		public bool Service(ISalad salad, string chefID) //Can change this into abstract order to extend scope for other dishes.
		{
			_servicedBy.Add(chefID);

			if (currentOrder.IsSame(salad))
			{
				customerHUD.HideProgress();
				TickableManager.Instance.Unsubscribe(this);
				gameObject.SetActive(false);
				Debug.Log("Hurray!CHOM CHOM CHOM CHOM....");
				return true;
			}
			else
			{
				_isAngry = true;
				Debug.Log("Wrong Order!Customer got angry at :" + chefID);
				return false;
			}
		}
		#endregion

	}
}

