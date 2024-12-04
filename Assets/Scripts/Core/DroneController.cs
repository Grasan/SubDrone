using UnityEngine;
using UnityEngine.InputSystem;

namespace SubDrone {
    public class DroneController : MonoBehaviour {
        #region Stats
        [Header("Stats")]
        [Tooltip("It's health... basicaly health.")]
        [SerializeField] private float droneIntegrity = 1;
        [Tooltip("The points the player earn from cololecting treasure, aswell as collecting mission related items.")]
        [SerializeField] private int points = 0;
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
        [Tooltip("Inverting the rotation in the Y axis")]
        [SerializeField] private bool inverted = true;
        [Space]
        #endregion
        #region Constants
        [Header("Constants")]
        [Tooltip("For calculating the dammage on collisions: (CurrentSpeed / MaxSpeed) * Constant")]
        [SerializeField] private float collisionConstant = 0.25f;
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
        private BoxCollider _col;
        private Rigidbody _rb;
        #endregion

        private void Awake() {
            //Component Setup
            try {
                _col = GetComponent<BoxCollider>();
            } catch {
                Debug.LogError("Could not find the player's BoxCollider Component!");
            }
            try {
                _rb = GetComponent<Rigidbody>();
            } catch {
                Debug.LogError("Could not find the player's RigidBody Component!");
            }

            // Velocity Reset
            currentVelocity = Vector3.zero;
            currentAngularRotation = Vector3.zero;
        }

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
            rotationAxis = inverted ? new Vector2(input.x, -input.y) : input;

        }
        public void Roll(InputAction.CallbackContext ctx) {
            rollAxis = ctx.ReadValue<float>();
        }

        private void Move() {
            Vector3 targetSpeed = new(0, elevationAxis * maxElevationSpeed, forwardAxis * maxForwardSpeed);

            if (targetSpeed.magnitude > 0.1f)
                currentVelocity = Vector3.MoveTowards(currentVelocity, targetSpeed, thrustAcceleration * Time.fixedDeltaTime);
            else
                currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, ThrustDeceleration * Time.fixedDeltaTime);

            // Applying movement.
            _rb.MovePosition(_rb.position + transform.TransformDirection(currentVelocity) * Time.fixedDeltaTime);
        }
        private void Rotate() {
            Vector3 targetAngularVelocity = new(
                -rotationAxis.y * maxRotationSpeed,
                rotationAxis.x * maxRotationSpeed,
                rollAxis * maxRotationSpeed
            );

            if (new Vector3(rotationAxis.y, rotationAxis.x, rollAxis).magnitude > 0.1f)
                currentAngularRotation = Vector3.MoveTowards(currentAngularRotation, targetAngularVelocity, rotationAcceleration * Time.fixedDeltaTime);
            else
                currentAngularRotation = Vector3.MoveTowards(currentAngularRotation, Vector3.zero, rotationDeceleration * Time.fixedDeltaTime);

            // Applying rotation.
            Quaternion deltaRotation = Quaternion.Euler(currentAngularRotation * Time.fixedDeltaTime);
            _rb.MoveRotation(_rb.rotation * deltaRotation);
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

        public void earnPoints(int value) {
            points += value;
        }

    }
}
