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
		
		[SerializeField] private float groundDrag;
		[SerializeField] private float _playerHeight;
		[SerializeField] private LayerMask _whatIsGround;
		[SerializeField] bool grounded;
		
		[SerializeField] float speed;
		[SerializeField] Transform orientation;
		
		private Rigidbody _rb;
		private Vector3 _moveDirection;
		
		void Awake()
		{
			_playerInput = GetComponent<PlayerInput>();
			_moveAction = _playerInput.actions.FindAction("Move");
			_rb = GetComponent<Rigidbody>();
			grounded = true;
		}

		void Update()
		{
			grounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _whatIsGround);
			
			if(grounded)
				_rb.drag = groundDrag;
			else
				_rb.drag = 0;
		}
		
		void FixedUpdate()
		{
			MovePlayer();
		}
		
		void MovePlayer()
		{
			Vector2 direction = _moveAction.ReadValue<Vector2>();
			_moveDirection = orientation.forward * direction.y + orientation.right * direction.x;
			_rb.AddForce(_moveDirection.normalized * speed * 10f, ForceMode.Force);
		}
		
		private void SpeedControl()
		{
			Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
			
			if(flatVel.magnitude > speed)
			{
				Vector3 limitedVel = flatVel.normalized * speed;
				_rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
			}
		}
	}
}

