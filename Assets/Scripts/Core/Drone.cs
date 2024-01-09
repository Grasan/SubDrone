using UnityEngine;

public class Drone : MonoBehaviour {
    [Header("Stats")]
    [Tooltip("It's health... basicaly health.")]
    [SerializeField] private float droneIntegrity = 1;
    [SerializeField] private int score = 0;
    [Space]
    [Header("Speed and Rotation")]
    [SerializeField] private float thrustAcceleration;
    [SerializeField] private float rotationAcceleration;
    [SerializeField] private float maxThrustSpeed;
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
    [SerializeField] private float thrustAxis;
    [SerializeField] private float elevationAxis;
    [SerializeField] private Vector2 rotationAxis;
    [SerializeField] private float rollAxis;

    // Components
    private DroneControls controls;
    private BoxCollider col;
    private Interaclable interactable;
    private Rigidbody rb;

    private void Awake () {
        controls = new DroneControls();
        col = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();

        // Controll setup
        // Thrust
        controls.Gameplay.Thrust.performed += ctx => thrustAxis = ctx.ReadValue<float>();
        controls.Gameplay.Thrust.canceled += ctx => thrustAxis = 0;

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

    private float CalcCollisionDammage() {
        float speedPercentage = currentForwardSpeed / maxThrustSpeed;

        return speedPercentage * collisionConstant;
    }

    private void checkIntegrity() {
        if (droneIntegrity <= 0) {
            OnDisable();
        }
    }

    // Movement functions
    private float CalcThrustInput(float current, float axisValue) {
        float target = maxThrustSpeed * axisValue;

        var maxAxisAndThust = axisValue >= 0.75f && current >= (maxThrustSpeed * 0.975f);
        var minAxisAndThrust = axisValue <= 0.25f && current <= (maxThrustSpeed * 0.025f);
        var maxAxisAndThustNeg = axisValue <= -0.9f && current <= (maxThrustSpeed * -0.975f); 
        var minAxisAndThrustNeg = axisValue >= -0.1f && current >= (maxThrustSpeed * -0.025f);

        // I dont really like this, but it's better then wating for ages to reach 0 or max-speeds
        // OBS: it's not working... why?
        if (maxAxisAndThust)
            return maxThrustSpeed;
        else if (maxAxisAndThustNeg)
            return maxThrustSpeed * -1;
        else if (minAxisAndThrust || minAxisAndThrustNeg)
            return 0.0f;
        else
            return Mathf.Lerp(current, target, thrustAcceleration * Time.deltaTime);

    }
    private float CalcRotationInput(float speed, float AxisValue) {
        float target = maxRotationSpeed * AxisValue;

        return Mathf.Lerp(speed, target, rotationAcceleration * Time.deltaTime);
    }

    private void Move() {
        // Thrust
        currentForwardSpeed = CalcThrustInput(currentForwardSpeed, thrustAxis);
        // Elevation
        currentElevationSpeed = CalcThrustInput(currentElevationSpeed, elevationAxis);

        // Rotaion
        if (inverted)
            currentPitchSpeed = CalcRotationInput(currentPitchSpeed, rotationAxis.y);
        else
            currentPitchSpeed = CalcRotationInput(currentPitchSpeed, (rotationAxis.y * -1));
        currentYawSpeed = CalcRotationInput(currentYawSpeed, rotationAxis.x);
        currentRollSpeed = CalcRotationInput(currentRollSpeed, rollAxis);

        // Applying movement and rotation.
        transform.Translate(new Vector3(0, 0, currentForwardSpeed) * Time.deltaTime);
        transform.Rotate(new Vector3(currentPitchSpeed, currentYawSpeed, currentRollSpeed) * Time.deltaTime);
    }

    // Controlls enabling
    private void OnEnable() { controls.Gameplay.Enable(); }
    private void OnDisable() { controls.Gameplay.Disable(); }
    
}
