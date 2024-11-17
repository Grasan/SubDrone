using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCharacter : MonoBehaviour {
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
    #endregion
    
    #region Components
    private Interactable interactable;
    private PlayerInput playerInput;
    private string currentControllSchema;
    private CharacterController characterController;
    public Transform groundcheck;
    public LayerMask groundMask;
    private Camera mainCamera;
    #endregion

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        mainCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;

        movementInputAxis = Vector2.zero;
        rotationInputAxis = Vector2.zero;
    }

    private void Update() {
        currentControllSchema = playerInput.currentControlScheme;

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
        characterController.Move(worldMovement * Time.deltaTime);

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    private void Look() {
        float sensitivity = currentControllSchema == "Gamepad" ? joystickSensitivity : mouseSensitivity;

        // Converting Vector2 values to floating values
        float pitchRotation = rotationInputAxis.y * sensitivity * Time.deltaTime; 
        float yawRotation = rotationInputAxis.x * sensitivity * Time.deltaTime;

        // Invert Y axis if controll is inverted.
        if (!inverted)
            pitchRotation = -pitchRotation;

        float currentPitch = mainCamera.transform.localEulerAngles.x;
        if (currentPitch > 180f)
            currentPitch -= 360f;

        // Clamping the pitch rotation to stop the player from overspining in the Y-axis (camera perspective)
        pitchRotation = Mathf.Clamp(currentPitch + pitchRotation, maxVerticleAngle, minVerticleAngle);

        // Applying rotations
        transform.Rotate(Vector3.up * yawRotation);
        mainCamera.transform.localRotation = Quaternion.Euler(pitchRotation, 0f, 0f);
    }
    #endregion

    public void Interact(InputAction.CallbackContext ctx) {
        if (ctx.phase == InputActionPhase.Performed) { 
            if (interactable != null)
                interactable.Interact();
            else
                Debug.Log("Nothing to interact with");
        }
    }

    public void Pause(InputAction.CallbackContext ctx) {
        if (ctx.phase == InputActionPhase.Performed) {

        }
    }

    public void StopGameEmulation(InputAction.CallbackContext ctx) {
        if (ctx.phase == InputActionPhase.Performed) {
            Application.Quit();
            EditorApplication.ExitPlaymode();
        }
    }
}