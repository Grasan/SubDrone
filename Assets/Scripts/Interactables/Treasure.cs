using SubDrone.Core;
using UnityEngine;

namespace SubDrone.Interactables {
    [ExecuteInEditMode]
    public class Treasure : Interactable {
        public TreasurePieceScriptableObject treasure;
        private GameObject _instantiatedPrefab;

        #if UNITY_EDITOR
        private TreasurePieceScriptableObject _previusTreasure;
        #endif

        private void Awake() {
            SetupTreasure();
        }

#if UNITY_EDITOR
        private void OnValidate() {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (this == null || _previusTreasure == treasure) return;
                SetupTreasure();
                _previusTreasure = treasure;
            };
        }
#endif

        protected void SetupTreasure() {
            if (_instantiatedPrefab != null) {
                if (Application.isPlaying) 
                    Destroy(_instantiatedPrefab);
                else
                    DestroyImmediate(_instantiatedPrefab);

                _instantiatedPrefab = null;
            }

            if (treasure == null) return;

            if (treasure.treasurePrefab == null) return;
            _instantiatedPrefab = Instantiate(treasure.treasurePrefab, transform);
            _instantiatedPrefab.transform.localPosition = Vector3.zero;
            _instantiatedPrefab.transform.localRotation = Quaternion.identity;
            _instantiatedPrefab.transform.localScale = Vector3.one;
        }

        public override void Interact() {
            _player.EarnPoints(treasure.points);

            if (treasure.pickupSound != null)
                AudioSource.PlayClipAtPoint(treasure.pickupSound, transform.position);

            if (treasure.type == TreasurePieceScriptableObject.TreasureType.Quest) {
                // Insert logic for Unity events handling quests and such.
            }

            Destroy(gameObject);
        }

    }
}