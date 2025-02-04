using UnityEngine;

namespace SubDrone {
    [ExecuteInEditMode]
    public class Treasure : Interactable {
        public TreasureSO treasureSO;
        private GameObject _instantiatedPrefab;

        #if UNITY_EDITOR
        private TreasureSO _previusTreasureSO;
        #endif

        private void Awake() {
            SetupTreasure();
        }

#if UNITY_EDITOR
        private void OnValidate() {
            UnityEditor.EditorApplication.delayCall += () => {
                if (this != null && _previusTreasureSO != treasureSO) {
                    SetupTreasure();
                    _previusTreasureSO = treasureSO;
                }
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

            if (treasureSO == null) return;

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