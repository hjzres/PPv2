using UnityEngine;

namespace Player{
	public class PlayerCam : MonoBehaviour
	{
		public float Sense;
		
		[SerializeField] Transform orientation;
		
		private float _xRotation;
		private float _yRotation;
		
		void Awake()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		
		void Update()
		{
			float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * Sense;
			float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * Sense;
			
			_yRotation += mouseX;
			_xRotation -= mouseY;
			
			_xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
			
			transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
			orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
		}
	}
}