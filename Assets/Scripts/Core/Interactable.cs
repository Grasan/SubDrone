using UnityEngine;

namespace SubDrone {
    public abstract class Interactable : MonoBehaviour {
        protected SphereCollider _trigger;
        [SerializeField] protected Player _player;

        private const string PLAYER_TAG = "Player";

        private void Start() {
            try {
                _trigger = GetComponent<SphereCollider>();
            } catch {
                Debug.LogError("Trigger not setup in " + name + "!");
            }
        }

        protected void OnTriggerEnter(Collider other) {
            if (other.tag != PLAYER_TAG)
                return;

            _player = other.GetComponent<Player>();
            _player.SetInteractable(this);
        }

        protected void OnTriggerExit(Collider other) {
            if (other.tag != PLAYER_TAG)
                return;

            _player.SetInteractable(null);
            _player = null;
        }

        public abstract void Interact();
    }
}
