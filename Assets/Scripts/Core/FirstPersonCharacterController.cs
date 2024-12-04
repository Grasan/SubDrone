using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SubDrone {
    public class FirstPersonCharacterController : MonoBehaviour {
        #region Movement Parameters
        [Header("Movement Parameters")]
        [SerializeField] private float maxMovementSpeed;
        [SerializeField] private float gravity;
        //[SerializeField] private float jumpHeight = 3f;   Probably not gonna be used
        bool isGrounded;
        [SerializeField] private float groundDistance;

        [Header("Rotation Parameters")]
        [Tooltip("The sensitivity over the camera rotation for the mouse")]
        [SerializeField] private float mouseSensitivity;
        [Tooltip("The sensitivity over the camera rotation for a joystick")]
        [SerializeField] private float joystickSensitivity;
        [Tooltip("Note: Rotation downwards (in the camera perspective) are positive")]
        [SerializeField] private float minVerticleAngle;
        [Tooltip("Note: Rotation upwards (in the camera perspective) are negative")]
        [SerializeField] private float maxVerticleAngle;
        [Tooltip("Inverting the rotation in the Y axis")]
        [SerializeField] private bool inverted;
        #endregion

        #region Current Axis
        [Space]
        [SerializeField] private Vector2 movementInputAxis;
        private Vector3 velocity;
        [SerializeField] private Vector2 rotationInputAxis;
        [SerializeField] private Vector3 currentPos;
        #endregion

        #region Components
        private PlayerInput _playerInput;
        private string currentControllSchema;
        private CharacterController _characterController;
        public Transform groundcheck;
        public LayerMask groundMask;
        private Camera _mainCamera;
        #endregion

        private void Awake() {
            _playerInput = GetComponent<PlayerInput>();
            _characterController = GetComponent<CharacterController>();
            _mainCamera = GetComponentInChildren<Camera>();
            Cursor.lockState = CursorLockMode.Locked;

            LoadPlayerControllValues();

            movementInputAxis = Vector2.zero;
            rotationInputAxis = Vector2.zero;
        }

        private void Update() {
            currentControllSchema = _playerInput.currentControlScheme;

            currentPos = transform.position;

            Move();
            Look();
        }

        #region Axis Input
        public void setMovementAxis(InputAction.CallbackContext ctx) {
            movementInputAxis = ctx.ReadValue<Vector2>();
        }
        public void setRotationAxis(InputAction.CallbackContext ctx) {
            rotationInputAxis = ctx.ReadValue<Vector2>();
        }
        #endregion

        #region Movement
        // Player movement and camera rotation
        private void Move() {
            isGrounded = Physics.CheckSphere(groundcheck.position, groundDistance, groundMask);
            if (isGrounded && velocity.y < 0)
                velocity.y = -2f;

            Vector3 horizontalMovement = new Vector3(movementInputAxis.x, 0, movementInputAxis.y);
            Vector3 worldMovement = transform.TransformDirection(horizontalMovement) * maxMovementSpeed;

            // Apply Movement
            _characterController.Move(worldMovement * Time.deltaTime);

            // Gravity
            velocity.y += gravity * Time.deltaTime;
            _characterController.Move(velocity * Time.deltaTime);
        }
        private void Look() {
            float sensitivity = currentControllSchema == "Gamepad" ? joystickSensitivity : mouseSensitivity;

            // Converting Vector2 values to floating values
            float pitchRotation = rotationInputAxis.y * sensitivity * Time.deltaTime;
            float yawRotation = rotationInputAxis.x * sensitivity * Time.deltaTime;

            // Invert Y axis if controll is inverted.
            if (!inverted)
                pitchRotation = -pitchRotation;

            float currentPitch = _mainCamera.transform.localEulerAngles.x;
            if (currentPitch > 180f)
                currentPitch -= 360f;

            // Clamping the pitch rotation to stop the player from overspining in the Y-axis (camera perspective)
            pitchRotation = Mathf.Clamp(currentPitch + pitchRotation, maxVerticleAngle, minVerticleAngle);

            // Applying rotations
            transform.Rotate(Vector3.up * yawRotation);
            _mainCamera.transform.localRotation = Quaternion.Euler(pitchRotation, 0f, 0f);
        }

        public void ResetPlayer(Transform resetPos) {
            _characterController.enabled = false;

            // Reseting the camera and player rotation
            _mainCamera.transform.eulerAngles = new Vector3(resetPos.rotation.x, 0f, 0f);
            transform.eulerAngles = new Vector3(0f, resetPos.rotation.y, 0f);

            // Reseting the velocity and position of the player
            velocity = Vector3.zero;
            transform.position = resetPos.position;

            _characterController.enabled = true;
        }
        #endregion

        private void LoadPlayerControllValues() {
            mouseSensitivity = PlayerPrefs.GetFloat("MouseSensetivity", 10f);
            joystickSensitivity = PlayerPrefs.GetFloat("JoystickSensetivity", 100f);
            inverted = Boolean.Parse(PlayerPrefs.GetString("Iverted", "false"));
        }
    }
}