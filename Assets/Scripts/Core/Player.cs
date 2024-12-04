using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SubDrone {
    public class Player : MonoBehaviour {
        private DroneController _droneController;
        private Interactable _interactable;

        private void Start() {
            try {
                _droneController = GetComponent<DroneController>();
            } catch {
                Debug.Log("DroneController script not found. Might be controlling FPS-Character" +
                    "\nIf not, Error has occured!");
                throw;
            }
        }

        public void SetInteractable(Interactable interactable) {
            this._interactable = interactable;
        }
        public void Interact(InputAction.CallbackContext ctx) {
            if (ctx.phase == InputActionPhase.Performed && _interactable != null)
                _interactable.Interact();
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
        public void EarnPoints(int points) {
            _droneController.earnPoints(points);
        }
    }
}
