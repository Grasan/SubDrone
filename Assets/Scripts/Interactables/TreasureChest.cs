using UnityEngine;

namespace SubDrone {
    public class TreasureChest : Treasure {
        private bool _isOpen = false;

        #if UNITY_EDITOR
        private void OnValidate() {
            if (treasureSO != null && treasureSO.type != TreasureSO.TreasureType.Chest) {
                Debug.LogWarning($"Warning: The Treasure assigned {name} in not of type 'Chest'. Please assign a valid TreasureSO of type 'Chest'.", this);
                treasureSO = null;
            }
        }
        #endif

        public override void Interact() {
            if (treasureSO == null) return;

            base.Interact();

            OpenLid();

            // Remove the trigger component.
            Destroy(_trigger);
        }

        /**
         * Instead of using Unity's animation system, 
         * the lid will be animated by changing the local rotation.
         */
        private void OpenLid() {

        }
    }
}