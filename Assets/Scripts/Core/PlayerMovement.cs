using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float thrustAcceleration = 5.0f;
    [SerializeField] private float rotationAcceleration = 2.0f;
    [SerializeField] private float maxRotationSpeed = 50.0f;
    [SerializeField] private float maxThrustSpeed = 200.0f;
    [SerializeField] private bool inverted = true;
    
    // current speeds
    private float currentForwardSpeed;
    private float currentElevationSpeed;
    private float currentYawSpeed;
    private float currentPitchSpeed;
    private float currentRollSpeed;
    
    // Axis
    private float thrustAxis;
    private float elevationAxis;
    private Vector2 rotationAxis;
    private float rollAxis;

    // Components
    private DroneControls controls;
    private Drone drone;

    private void Awake () {
        controls = new DroneControls();
        drone = GetComponent<Drone>();

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
        controls.Gameplay.Interaction.performed += ctx => drone.Interact();
    }

    // Update is called once per frame
    void Update() {
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
        transform.Translate(new Vector3(0, currentElevationSpeed, currentForwardSpeed) * Time.deltaTime);
        transform.Rotate(new Vector3(currentPitchSpeed, currentYawSpeed, currentRollSpeed) * Time.deltaTime);
    }

    private float CalcThrustInput(float speed, float axisValue) {
        float target = maxThrustSpeed * axisValue;

        return Mathf.Lerp(speed, target, thrustAcceleration * Time.deltaTime);
    }

    private float CalcRotationInput(float speed, float AxisValue) {
        float target = maxRotationSpeed * AxisValue;

        return Mathf.Lerp(speed, target, rotationAcceleration * Time.deltaTime);
    }

    private void OnEnable() {
        controls.Gameplay.Enable();
    }

    private void OnDisable() { 
        controls.Gameplay.Disable();
    }
}
