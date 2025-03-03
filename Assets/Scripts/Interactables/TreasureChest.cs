using UnityEngine;

namespace SubDrone.Interactables {
    [ExecuteInEditMode]
    public class TreasureChest : Treasure<TreasureChestScriptableObject> {
        private bool _isOpen;
        
        [SerializeField] private AnimationCurve _openCurve;

        protected override void Awake() {
            base.Awake();
            if (_isOpen) OpenLid();
        }

        public override void Interact() {
            Destroy(_trigger);
            
            _player.EarnPoints(treasure.points);
            if (treasure.obtainSound != null) 
                AudioSource.PlayClipAtPoint(treasure.obtainSound, transform.position);
            
            OpenLid();
        }

        /**
         * Instead of using Unity's animation system, 
         * the lid will be animated by changing the local rotation with the animationCurve variable.
         */
        private void OpenLid() {
            if (!_isOpen)
                _isOpen = true;
        }
    }
}