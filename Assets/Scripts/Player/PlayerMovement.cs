using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
	public class PlayerMovement : NetworkBehaviour
	{
		PlayerInput _playerInput;
		InputAction _moveAction;
		InputAction _jumpAction;
		
		[Header("Move")]
		[SerializeField] float speed;
		[SerializeField] private float groundDrag;

		
		[Header("Jump")]
		[SerializeField] private float jumpForce;
		[SerializeField] private float jumpCooldown;
		[SerializeField] private float airMultiplier;
		[SerializeField] private bool readyToJump;
		
		[SerializeField] private float playerHeight;
		[SerializeField] private LayerMask whatIsGround;
		[SerializeField] bool isGrounded;
		
		[SerializeField] Transform orientation;
		
		private Rigidbody _rb;
		private Vector3 _moveDirection;
		
		void Awake()
		{
			if(!IsOwner) return;
			
			_playerInput = GetComponent<PlayerInput>();
			_rb = GetComponent<Rigidbody>();
			
			_moveAction = _playerInput.actions.FindAction("Move");
			_jumpAction = _playerInput.actions.FindAction("Jump");
			
			_rb.freezeRotation = true;
			
			readyToJump = true;
			isGrounded = true;
		}

		void Update()
		{
			if(!IsOwner) return;
			
			isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
			
			if(isGrounded)
				_rb.drag = groundDrag;
			else
				_rb.drag = 0;
			
			if(JumpInput() && readyToJump && isGrounded)
			{
				readyToJump = false;
							
				Jump();
				
				Invoke(nameof(ResetJump), jumpCooldown);
			}
		}
		
		void FixedUpdate()
		{
			if(!IsOwner) return;
			
			MovePlayer();
		}
		
		void MovePlayer()
		{
			Vector2 direction = _moveAction.ReadValue<Vector2>();
			_moveDirection = orientation.forward * direction.y + orientation.right * direction.x;
			
			
			if(isGrounded)
				_rb.AddForce(_moveDirection.normalized * speed * 10f, ForceMode.Force);
			else
				_rb.AddForce(_moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);
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
		
		private void Jump()
		{
			_rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
			
			_rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
		}
		
		private void ResetJump()
		{
			readyToJump = true;
		}
		
		private bool JumpInput()
		{
			return _jumpAction.ReadValue<float>() == 1f;
		}
	}
}

