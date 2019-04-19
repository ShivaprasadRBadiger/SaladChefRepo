using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(SpriteRenderer))]
	public class PlayerMovement : MonoBehaviour, IFixedTickable, ITickable, ILateTickable
	{

		Rigidbody2D rigidbody;
		Animator animator;
		SpriteRenderer spriteRenderer;



		[SerializeField]
		InputBindings inputBindings;

		Vector2 velocity = Vector2.zero;
		Vector2 lastDirection = Vector2.zero;


		[SerializeField]
		float movementSpeed = 1;

		GameManager gameManager;
		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody2D>();
			animator = GetComponent<Animator>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			gameManager = GameManager.Instance;

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
			velocity = GetDirection(inputBindings);
		}

		public void FixedTick()
		{
			velocity = velocity * Time.fixedDeltaTime * movementSpeed;
			rigidbody.MovePosition(((Vector2)transform.position + velocity));
		}

		public void LateTick()
		{
			animator.SetFloat("VelocityX", velocity.x);
			animator.SetFloat("VelocityY", velocity.y);
			animator.SetFloat("Speed", velocity.sqrMagnitude);
			if (velocity.sqrMagnitude > 0.5)
			{
				spriteRenderer.flipX = velocity.x > 0 ? false : true;
				animator.SetFloat("DirectionX", velocity.x);
				animator.SetFloat("DirectionY", velocity.y);
			}
		}
		private Vector2 GetDirection(InputBindings inputBindings, bool normalized = true)
		{
			Vector2 result = Vector2.zero;
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

	}
}

