using UnityEngine;

namespace SubDrone {
    public class TreasureChest : Treasure {
        private bool _isOpen = false;
        public GameObject lid;

        [SerializeField] private AnimationCurve _lidAnimationCurve;

        #if UNITY_EDITOR
        private void OnValidate() {
            if (treasureSO == null)
                return;
            
            if(treasureSO.type != TreasureSO.TreasureType.Chest) {
                Debug.LogWarning($"Warning: The Treasure assigned {name} in not of type 'Chest'. Please assign a valid TreasureSO of type 'Chest'.", this);
                treasureSO = null;
            } else {
                SetupTreasure();
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