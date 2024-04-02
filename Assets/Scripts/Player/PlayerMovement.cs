using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Player
{
	public class PlayerMovement : MonoBehaviour
	{
		PlayerInput _playerInput;
		InputAction _moveAction;
		
		[SerializeField] float speed;
		[SerializeField] Transform orientation;
		
		private Rigidbody _rb;
		private Vector3 _moveDirection;
		
		void Start()
		{
			_playerInput = GetComponent<PlayerInput>();
			_moveAction = _playerInput.actions.FindAction("Move");
			_rb = GetComponent<Rigidbody>();
		}

		void Update()
		{
			MovePlayer();
		}
		
		void MovePlayer()
		{
			Vector2 direction = _moveAction.ReadValue<Vector2>();
			_moveDirection = orientation.forward * direction.y + orientation.right * direction.x;
			_rb.AddForce(_moveDirection.normalized * speed * 10f, ForceMode.Force);
		}
	}
}

