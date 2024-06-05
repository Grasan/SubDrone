using UnityEngine;
using UnityEngine.InputSystem;

public class Drone : MonoBehaviour {
    #region Stats
    [Header("Stats")]
    [Tooltip("It's health... basicaly health.")]
    [SerializeField] private float droneIntegrity = 1;
    [Tooltip("The points the player earn from cololecting treasure, aswell as collecting mission related items.")]
    [SerializeField] private int score = 0;
    [Space]
    #endregion
    #region Movement Parameters
    [Header("Speed Parameters")]
    [SerializeField] private float thrustAcceleration;
    [SerializeField] private float ThrustDeceleration;
    [SerializeField] private float maxForwardSpeed;
    [SerializeField] private float maxElevationSpeed;
    [Header("Rotation Parameters")]
    [SerializeField] private float rotationAcceleration;
    [SerializeField] private float rotationDeceleration;
    [SerializeField] private float maxRotationSpeed;
    [Tooltip("Inverting the rotation the Y axis")]
    [SerializeField] private bool inverted = true;
    [Space]
    #endregion
    #region Constants
    [Header("Constants")]
    //[Tooltip("For calculating the dammage on collisions: (CurrentSpeed / MaxSpeed) * Constant")]
    //[SerializeField] private float collisionConstant = 0.25f;
    #endregion
    #region Current Speeds
    [Space]
    [Header("Current Speeds")]
    [SerializeField] private Vector3 currentVelocity;
    [SerializeField] private Vector3 currentAngularRotation;
    #endregion
    #region Axis
    [Space]
    [Header("Axis values")]
    [SerializeField] private float forwardAxis;
    [SerializeField] private float elevationAxis;
    [SerializeField] private Vector2 rotationAxis;
    [SerializeField] private float rollAxis;
    #endregion
    #region Components
    private DroneControls controls;
    private BoxCollider col;
    private Interaclable interactable;
    private Rigidbody rb;
    #endregion

    private void Awake() {
        //Component Setup
        controls = new DroneControls();
        col = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();

        // Velocity Reset
        currentVelocity = Vector3.zero;
        currentAngularRotation = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate() {
        Move();
        Rotate();
    }

    #region Movement
    public void AccelerateForward(InputAction.CallbackContext ctx) {
        forwardAxis = ctx.ReadValue<float>();
    }
    public void AccelerateElevation(InputAction.CallbackContext ctx) {
        elevationAxis = ctx.ReadValue<float>();
    }
    public void Rotation(InputAction.CallbackContext ctx) {
        Vector2 input = ctx.ReadValue<Vector2>();

        if (inverted)
            rotationAxis = new Vector2(input.x, -input.y);
        else
            rotationAxis = input;
    }
    public void Roll(InputAction.CallbackContext ctx) {
        rollAxis = ctx.ReadValue<float>();
    }

    private void Move() {
        Vector3 targetSpeed = new Vector3(0, elevationAxis * maxElevationSpeed, forwardAxis * maxForwardSpeed);

        if (targetSpeed.magnitude > 0.1f)
            currentVelocity = Vector3.MoveTowards(currentVelocity, targetSpeed, thrustAcceleration * Time.fixedDeltaTime);
        else
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, ThrustDeceleration * Time.fixedDeltaTime);

        rb.MovePosition(rb.position + transform.TransformDirection(currentVelocity) * Time.fixedDeltaTime);
    }
    private void Rotate() {
        
        Vector3 targetAngularVelocity = new Vector3(
            -rotationAxis.y * maxRotationSpeed,
            rotationAxis.x * maxRotationSpeed,
            rollAxis * maxRotationSpeed
        );

        if (rotationAxis.magnitude > 0.1f && (rollAxis <= 0.1f && rollAxis >= -0.1f))
            currentAngularRotation = Vector3.MoveTowards(currentAngularRotation, targetAngularVelocity, rotationAcceleration * Time.fixedDeltaTime);
        else
            currentAngularRotation = Vector3.MoveTowards(currentAngularRotation, Vector3.zero, rotationDeceleration * Time.fixedDeltaTime);

        // Applying rotation.
        Quaternion deltaRotation = Quaternion.Euler(currentAngularRotation * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    private bool noAxisInput(float velocity, float axis) {
        if (axis == 0 && velocity <= 0.05f && velocity >= -0.05f)
            return true;
        else
            return false;
    }
    #endregion

    #region Interaction
    // FIX: Gets invoked three times (onPress, onHold and onRealese)
    public void Interact() {
        if (interactable != null)
            interactable.Interact();
        else
            Debug.Log("Nothing to interact with.");
    }
    public void SetInteractable(Interaclable interaclable) {
        this.interactable = interaclable;
    }
    public void EarnPoints(int points) {
        score += points;
    }
    #endregion

    #region Health Related
    /*public void TakeDammage(float dammage) {
        droneIntegrity -= dammage;
        checkIntegrity();
    }
    private float CalcCollisionDammage() {
        float speedPercentage = currentForwardSpeed / maxThrustSpeed;

        return speedPercentage * collisionConstant;
    }
    private void checkIntegrity() {
        if (droneIntegrity <= 0) {
            OnDisable();
        }
    }*/
    #endregion

}
