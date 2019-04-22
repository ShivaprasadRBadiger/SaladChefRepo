using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(SpriteRenderer))]
	[RequireComponent(typeof(IPlayerInputBindings))]
	public class PlayerMovement : MonoBehaviour, IFixedTickable, ITickable, IPlayerMovement
	{
		private TickableManager gameManager;
		private Rigidbody2D rigidbody;
		private IPlayerInputBindings playerInputBindings;
		private InputBindings playerInput;
		private Vector2 velocity = Vector2.zero;
		private Vector2 lastDirection = Vector2.zero;
		private bool canControl = true;

		//TODO:Re-factor animations out of this class
		private Animator animator;
		private SpriteRenderer spriteRenderer;


		[Header("Movement Speed Controls")]
		[SerializeField]
		private float movementSpeed = 1;
		[SerializeField]
		private float boostedMultiplier = 2;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody2D>();
			gameManager = TickableManager.Instance;
			playerInputBindings = GetComponent<IPlayerInputBindings>();
			playerInput = playerInputBindings.GetInputBindings();
			if (playerInput == null)
			{
				Debug.LogWarning("Input bindings now assigned!");
			}
			//TODO:Re-factor Animation out of this class
			animator = GetComponent<Animator>();
			spriteRenderer = GetComponent<SpriteRenderer>();

			gameManager.Subscribe(this);
			gameManager.Subscribe(this);
		}


		private void OnDestroy()
		{
			gameManager.Unsubscribe(this);
			gameManager.Unsubscribe(this);
		}

		public void Tick()
		{
			if (playerInput != null)
			{
				velocity = GetDirection(playerInput);
			}
			//TODO: Should be taken out of this class
			#region Animation
			animator.SetFloat("VelocityX", velocity.x);
			animator.SetFloat("VelocityY", velocity.y);
			animator.SetFloat("Speed", velocity.sqrMagnitude);
			if (velocity.sqrMagnitude > 0.5)
			{
				spriteRenderer.flipX = velocity.x > 0 ? false : true;
				animator.SetFloat("DirectionX", velocity.x);
				animator.SetFloat("DirectionY", velocity.y);
			}
			#endregion
		}

		public void FixedTick()
		{
			velocity = velocity * Time.fixedDeltaTime * movementSpeed;
			rigidbody.MovePosition(((Vector2)transform.position + velocity));
		}

		private Vector2 GetDirection(InputBindings inputBindings, bool normalized = true)
		{
			Vector2 result = Vector2.zero;
			if (!canControl)
			{
				return result;
			}
			if (Input.GetKey(inputBindings.Up))
			{
				result.y = 1;
			}
			else if (Input.GetKey(inputBindings.Down))
			{
				result.y = -1;
			}
			if (Input.GetKey(inputBindings.Right))
			{
				result.x = 1;
			}
			else if (Input.GetKey(inputBindings.Left))
			{
				result.x = -1;
			}
			if (normalized)
			{
				return result.normalized;
			}
			else
			{
				return result;
			}
		}

		public void BoostSpeed()
		{
			movementSpeed = 2;
		}
		public void DisableControls()
		{
			canControl = false;
		}
		public void EnableControls()
		{
			canControl = true;
		}
	}
}

