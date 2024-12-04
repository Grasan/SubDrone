using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    protected SphereCollider _trigger;
    [SerializeField] protected Drone _drone;
    [SerializeField] protected FirstPersonCharacter _fpsCharacter;

    private const string PLAYER_TAG = "Player";

    private void Start() {
        try {
            _trigger = GetComponent<SphereCollider>();
        } catch {
            Debug.LogError("Trigger not setup in " + name + "!");
        }

    }

    private void setPlayerObject(Collider col) {
        if (col.TryGetComponent(out FirstPersonCharacter fpsCharacter))
            _fpsCharacter = fpsCharacter;
        else if (col.TryGetComponent(out Drone drone))
            _drone = drone;
    }
    private void clearPlayerObjects() {
        _fpsCharacter = null;
        _drone = null;
    }

    protected void OnTriggerEnter(Collider other) {
        if (other.tag != PLAYER_TAG)
            return;

        setPlayerObject(other);
            
        if (_fpsCharacter != null)
            _fpsCharacter.SetInteractable(this);
        else if (_drone != null)
            _drone.SetInteractable(this);
    }

    protected void OnTriggerExit(Collider other) {
        if (other.tag != PLAYER_TAG)
            return;

        if (_fpsCharacter != null)
            _fpsCharacter.SetInteractable(null);
        else if (_drone != null)
            _drone.SetInteractable(null);

        clearPlayerObjects();
    }

    public abstract void Interact();
}
