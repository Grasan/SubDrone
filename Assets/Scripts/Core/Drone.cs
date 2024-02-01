using UnityEngine;

public class Drone : MonoBehaviour {
    [Header("Stats")]
    [Tooltip("It's health... basicaly health.")]
    [SerializeField] private float droneIntegrity = 1;
    [Tooltip("The points the player earn from cololecting treasure, aswell as collecting quest related items.")]
    [SerializeField] private int score = 0;
    [Space]
    [Header("Speed and Rotation")]
    [SerializeField] private float forwardAcceleration;
    [SerializeField] private float elevationAcceleration;
    [SerializeField] private float rotationAcceleration;
    [SerializeField] private float maxForwardSpeed;
    [SerializeField] private float maxElevationSpeed;
    [SerializeField] private float maxRotationSpeed;
    [Tooltip("Inverting the rotation the Y axis")]
    [SerializeField] private bool inverted = true;
    [Space]
    [Header("Constants")]
    [Tooltip("For calculating the dammage on collisions: (CurrentSpeed / MaxSpeed) * Constant")]
    [SerializeField] private float collisionConstant = 0.25f;

    // current speeds
    [Space]
    [Header("Current Speeds")]
    [SerializeField] private float currentForwardSpeed;
    [SerializeField] private float currentElevationSpeed;
    [SerializeField] private float currentYawSpeed;
    [SerializeField] private float currentPitchSpeed;
    [SerializeField] private float currentRollSpeed;

    // Axis
    [Space]
    [Header("Axis values")]
    [SerializeField] private float forwardAxis;
    [SerializeField] private float elevationAxis;
    [SerializeField] private Vector2 rotationAxis;
    [SerializeField] private float rollAxis;

    // Components
    private DroneControls controls;
    private BoxCollider col;
    private Interaclable interactable;
    private Rigidbody rb;

    private void Awake() {
        controls = new DroneControls();
        col = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();

        currentForwardSpeed = 0f;
        currentElevationSpeed = 0f;
        currentYawSpeed = 0f;
        currentPitchSpeed = 0f;
        currentRollSpeed = 0f;

        // Controll setup
        // Thrust
        controls.Gameplay.Thrust.performed += ctx => forwardAxis = ctx.ReadValue<float>();
        controls.Gameplay.Thrust.canceled += ctx => forwardAxis = 0;

        // Elevation
        controls.Gameplay.Elevation.performed += ctx => elevationAxis = ctx.ReadValue<float>();
        controls.Gameplay.Elevation.canceled += ctx => elevationAxis = 0;

        // Rotation
        controls.Gameplay.Rotation.performed += ctx => rotationAxis = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotation.canceled += ctx => rotationAxis = Vector2.zero;
        controls.Gameplay.Roll.performed += ctx => rollAxis = ctx.ReadValue<float>();
        controls.Gameplay.Roll.canceled += ctx => rollAxis = 0;

        // Interaction
        controls.Gameplay.Interaction.performed += ctx => Interact();
    }

    // Update is called once per frame
    void Update() {
        Move();

    }

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

    public void TakeDammage(float dammage) {
        droneIntegrity -= dammage;
        checkIntegrity();
    }

    /*private float CalcCollisionDammage() {
        float speedPercentage = currentForwardSpeed / maxThrustSpeed;

        return speedPercentage * collisionConstant;
    }*/

    private void checkIntegrity() {
        if (droneIntegrity <= 0) {
            OnDisable();
        }
    }

    // Movement functions
    private float CalcThrustInput(float currentSpeed, float maxSpeed, float acceleration, float axisValue) {
        float target = maxSpeed * axisValue,
            newVelocity = Mathf.Lerp(currentSpeed, target, acceleration * Time.deltaTime);

        if (!noAxisInput(newVelocity, axisValue))
            return newVelocity;
        else
            return 0;
    }

    private bool noAxisInput(float velocity, float axis) {
        if (axis == 0 && velocity <= 0.05f && velocity >= -0.05f)
            return true;
        else
            return false;
    }

    private void Move() {
        // Thrust
        currentForwardSpeed = CalcThrustInput(currentForwardSpeed, maxForwardSpeed, forwardAcceleration, forwardAxis);

        // Elevation
        currentElevationSpeed = CalcThrustInput(currentElevationSpeed, maxElevationSpeed, elevationAcceleration, elevationAxis);

        // Rotaion Y
        if (inverted)
            currentPitchSpeed = CalcThrustInput(currentPitchSpeed, maxRotationSpeed, rotationAcceleration, rotationAxis.y);
        else
            currentPitchSpeed = CalcThrustInput(currentPitchSpeed, maxRotationSpeed, rotationAcceleration, (rotationAxis.y * -1));

        // Rotatio X
        currentYawSpeed = CalcThrustInput(currentYawSpeed, maxRotationSpeed, rotationAcceleration, rotationAxis.x);

        // Roll
        currentRollSpeed = CalcThrustInput(currentRollSpeed, maxRotationSpeed, rotationAcceleration, rollAxis);

        // Applying movement and rotation.
        transform.Translate(new Vector3(0, currentElevationSpeed, currentForwardSpeed) * Time.deltaTime);
        transform.Rotate(new Vector3(currentPitchSpeed, currentYawSpeed, currentRollSpeed) * Time.deltaTime);
    }

    // Controlls enabling
    private void OnEnable() { controls.Gameplay.Enable(); }
    private void OnDisable() { controls.Gameplay.Disable(); }

}
