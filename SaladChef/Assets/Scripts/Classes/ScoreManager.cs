
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SaladChef
{
	public class ScoreManager : MonoBehaviour, ICustomerObserver, ITickable
	{
		private const string TimePrefix = "TIME:";
		private const string ScorePrefix = "SCORE:";

		[SerializeField]
		private int startingTime = 360;

		[SerializeField]
		private int scoreOnService = 25;
		[SerializeField]
		private int negativeScoreOnNoService = 25;
		[SerializeField]
		private int angryNegativeMultiplier = 2;


		[SerializeField]
		Text player1ScoreText, player2ScoreText;
		[SerializeField]
		Text player1TimeText, player2TimeText;

		[SerializeField]
		NpcCustomerBehaviour[] custFeedBackProviders;

		[SerializeField]
		Text gameOver;

		[SerializeField]
		Button quitButton;
		[SerializeField]
		Button resumeButton;

		private int _p1Score;
		private int p1Score
		{
			set
			{

				if (value < 0)
				{
					_p1Score = 0;
				}
				else
				{
					_p1Score = value;
				}
				if (player1ScoreText)
					player1ScoreText.text = ScorePrefix + _p1Score.ToString();
			}
			get
			{
				return _p1Score;
			}
		}

		private int _p2Score;
		private int p2Score
		{
			set
			{
				if (value < 0)
				{
					_p2Score = 0;
				}
				else
				{
					_p2Score = value;
				}
				if (player2ScoreText)
				{
					player2ScoreText.text = ScorePrefix + _p2Score.ToString();
				}
			}
			get
			{
				return _p2Score;
			}
		}

		private int _p1Time;
		private int p1Time
		{
			set
			{
				if (player1TimeText)
					player1TimeText.text = TimePrefix + value.ToString();
				_p1Time = value;
			}
			get
			{
				return _p1Time;
			}
		}

		private int _p2Time;
		private int p2Time
		{
			set
			{
				if (player2TimeText)
					player2TimeText.text = TimePrefix + value.ToString();
				_p2Time = value;
			}
			get
			{
				return _p2Time;
			}
		}


		private void Awake()
		{
			TickableManager.Instance.Subscribe(this);
			var NpcCustomers = FindObjectsOfType<NpcCustomerBehaviour>();
			for (int i = 0; i < custFeedBackProviders.Length; i++)
			{
				var feedbackInterface = (ISubject)custFeedBackProviders[i];
				feedbackInterface.Register(this);
			}
			quitButton.onClick.AddListener(QuitGame);
			resumeButton.onClick.AddListener(ResumeGame);
		}

		private void QuitGame()
		{
			Application.Quit();
		}

		private void OnEnable()
		{
			ResetScoreAndTime();
		}
		public void ResetScoreAndTime()
		{
			p1Time = startingTime;
			p2Time = startingTime;
			p1Score = 0;
			p2Score = 0;
		}

		public void OnCustomerLeft(INpcCustomer leavingCustomer)
		{
			switch (leavingCustomer.satisfaction)
			{
				case Satisfaction.Excellent:
					//Spawn Power
					break;
				case Satisfaction.Good:
					if (leavingCustomer.servicedBy.Contains(GameTags.P1))
					{
						p1Score += scoreOnService;
					}
					else
					{
						p2Score += scoreOnService;
					}
					break;
				case Satisfaction.Bad:
					if (!leavingCustomer.isAngry)
					{
						p1Score -= negativeScoreOnNoService;
						p2Score -= negativeScoreOnNoService;
					}
					else
					{
						if (leavingCustomer.servicedBy.Contains(GameTags.P1))
							p1Score -= negativeScoreOnNoService * angryNegativeMultiplier;
						if (leavingCustomer.servicedBy.Contains(GameTags.P2))
							p2Score -= negativeScoreOnNoService * angryNegativeMultiplier;
					}
					break;
			}
		}
		float secondTimer = 1f;
		public void Tick()
		{
			if (secondTimer > 0)
			{
				secondTimer -= Time.deltaTime;
			}
			else
			{
				secondTimer = 1f;
				//Second elapsed
				p1Time--;
				p2Time--;

				if (p1Time <= 0)
				{
					//End Game for P1
					GameObject.FindGameObjectWithTag(GameTags.P1).SetActive(false);
				}
				if (p2Time <= 0)
				{
					//End Game for P2
					GameObject.FindGameObjectWithTag(GameTags.P2).SetActive(false);
				}

				if (p1Time <= 0 && p2Time <= 0)
				{
					Time.timeScale = 0;
					gameOver.transform.parent.gameObject.SetActive(true);
					if (p1Score != p2Score)
						gameOver.text = "Player " + (p1Score > p2Score ? " P1 " : " P2 ") + " Wins";
					else
						gameOver.text = "GameOver with Draw";
				}
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Time.timeScale = 0;
				gameOver.text = "GAME PAUSED";
				gameOver.transform.parent.gameObject.SetActive(true);
			}
		}

		private void ResumeGame()
		{
			Time.timeScale = 1;
			gameOver.text = "GAME OVER";
			gameOver.transform.parent.gameObject.SetActive(false);
		}
	}
}
