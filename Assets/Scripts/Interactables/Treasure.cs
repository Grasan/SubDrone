using UnityEngine;

namespace SubDrone {
    [ExecuteInEditMode]
    public class Treasure : Interactable {
        public TreasureSO treasureSO;
        private GameObject _instantiatedPrefab;

        private void Awake() {
            SetupTreasure();
        }

        #if UNITY_EDITOR
        private void OnValidate() {
            SetupTreasure();
        }
        #endif

        private void SetupTreasure() {
            if (treasureSO == null) return;

            if (_instantiatedPrefab != null) 
                DestroyImmediate(_instantiatedPrefab);

            if (treasureSO.treasurePrefab != null) {
                _instantiatedPrefab = Instantiate(treasureSO.treasurePrefab, transform);
                _instantiatedPrefab.transform.localPosition = Vector3.zero;
                _instantiatedPrefab.transform.localRotation = Quaternion.identity;
                _instantiatedPrefab.transform.localScale = Vector3.one;
            }
        }

        public override void Interact() {
            _player.EarnPoints(treasureSO.points);

            if (treasureSO.pickupSound != null)
                AudioSource.PlayClipAtPoint(treasureSO.pickupSound, transform.position);

            if (treasureSO.isQuestItem) {
                // Insert logic for Unity events handling quests and such.
            }

            Destroy(gameObject);
        }

    }
}