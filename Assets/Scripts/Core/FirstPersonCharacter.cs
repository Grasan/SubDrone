using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCharacter : MonoBehaviour {
    #region Movement Parameters
    [Header("Movement Parameters")]
    [SerializeField] private float maxMovementSpeed;
    [Header("Rotation Parameters")]
    [SerializeField] private float maxRotationSpeed;
    [SerializeField] private float minVerticleAngle;
    [SerializeField] private float maxVerticleAngle;
    [Tooltip("Inverting the rotation in the Y axis")]
    [SerializeField] private bool inverted = true;
    #endregion
    #region Axis
    [Space]
    [Header("Axis values")]
    private Vector2 movementAxis;
    private Vector2 rotationAxis;
    private float currVerticalRotation = 0f;
    #endregion
    #region Current Speeds
    private Vector3 currentSpeed;
    private Vector3 currentAngularRotation;
    #endregion
    #region Components
    private Interactable interactable;
    private Rigidbody rb;
    private Camera mainCamera;
    #endregion

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        mainCamera = GetComponentInChildren<Camera>();

        movementAxis = Vector2.zero;
        rotationAxis = Vector2.zero;
    }

    void FixedUpdate() {
        Move();
        Look();
    }

    #region Movement
    public void setMovementAxis(InputAction.CallbackContext ctx) {
        movementAxis = ctx.ReadValue<Vector2>();   
    }
    public void setRotationAxis(InputAction.CallbackContext ctx) { 
        Vector2 input = ctx.ReadValue<Vector2>();
        rotationAxis = inverted ? new (input.x, -input.y) : input;
    }

    private void Move() {
        Vector3 targetVelocity = transform.TransformDirection(new Vector3(movementAxis.x, 0, movementAxis.y) * maxMovementSpeed);

        rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
    }
    private void Look() {
        transform.Rotate(Vector3.up * rotationAxis.x * maxRotationSpeed * Time.deltaTime);

        currVerticalRotation += rotationAxis.y * maxRotationSpeed * Time.deltaTime;
        currVerticalRotation = Mathf.Clamp(currVerticalRotation, minVerticleAngle, maxVerticleAngle);

        mainCamera.transform.localEulerAngles = new Vector3(-currVerticalRotation, 0, 0);
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