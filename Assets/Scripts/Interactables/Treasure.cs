using UnityEngine;

namespace SubDrone.Interactables {
    [ExecuteInEditMode]
    public abstract class Treasure<T> : TreasureBase where T : BaseTreasureScriptableObject {
        [ExecuteInEditMode]
        public T treasure;
        private GameObject _instantiatedPrefab;

        #if UNITY_EDITOR
        private T _previusTreasure;
        #endif

        protected virtual void Awake() {
            SetupTreasure();
        }

        #if UNITY_EDITOR
        protected void OnValidate() {
            UnityEditor.EditorApplication.delayCall += () => {
                if (this == null || _previusTreasure == treasure) return;
                
                SetupTreasure();
                _previusTreasure = treasure;
            };
        }
        #endif

        private void SetupTreasure() {
            if (_instantiatedPrefab != null) {
                if (Application.isPlaying) Destroy(_instantiatedPrefab);
                else DestroyImmediate(_instantiatedPrefab);

                _instantiatedPrefab = null;
            }

            if (treasure == null || treasure.prefab == null) return;
            _instantiatedPrefab = Instantiate(treasure.prefab, transform);
            _instantiatedPrefab.transform.localPosition = Vector3.zero;
            _instantiatedPrefab.transform.localRotation = Quaternion.identity;
            _instantiatedPrefab.transform.localScale = Vector3.one;
        }

        public override int GetPoints() {
            return treasure.points;
        }
    }
}