using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
	public class PlayerMovement : MonoBehaviour
	{
		PlayerInput playerInput;
		InputAction moveAction; 
		// Start is called before the first frame update
		void Start()
		{
			playerInput = GetComponent<PlayerInput>();
			moveAction = playerInput.actions.FindAction("Move");
		}

		// Update is called once per frame
		void Update()
		{
			MovePlayer();
		}
		
		void MovePlayer()
		{
			Debug.Log(moveAction.ReadValue<Vector2>());
		}
	}
}

