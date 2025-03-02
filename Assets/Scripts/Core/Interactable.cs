using UnityEngine;

namespace SubDrone.Core {
    public abstract class Interactable : MonoBehaviour {
        protected SphereCollider _trigger;
        protected Player _player;

        private const string PLAYER_TAG = "Player";

        private void Start() {
            _trigger = GetComponent<SphereCollider>();
            if (_trigger == null) 
                Debug.LogError($"Trigger not setup in {name}!");
            
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag(PLAYER_TAG))
                return;

            _player = other.GetComponent<Player>();
            _player.SetInteractable(this);
        }

        private void OnTriggerExit(Collider other) {
            if (!other.CompareTag(PLAYER_TAG))
                return;

            _player.SetInteractable(null);
            _player = null;
        }

        public abstract void Interact();
    }
}
