using UnityEngine;

namespace SubDrone.Interactables {
    public class TreasureChest : Treasure
    {
        public TreasureChestScriptableObject chest;
        
        private bool _isOpen = false;
        public GameObject lid;

        [SerializeField] private AnimationCurve _lidAnimationCurve;

        #if UNITY_EDITOR
        private void OnValidate() {
            if (treasure == null)
                return;
        }
        #endif

        public override void Interact() {
            if (treasure == null) return;

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