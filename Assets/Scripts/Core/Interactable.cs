using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    protected SphereCollider trigger;
    protected Drone player;

    private const string playerTag = "Player";

    private void Start() {
        try {
            trigger = GetComponent<SphereCollider>();
        } catch {
            Debug.LogError("Trigger not setup in " + name + "!");
        }
        try {
            player = FindObjectOfType<Drone>();
        } catch {
            Debug.LogError("Player not found!");
        }
    }

    protected void OnTriggerEnter(Collider other) {
        if (other.CompareTag(playerTag) && player != null) {
            player.SetInteractable(this);
        }
    }

    protected void OnTriggerExit(Collider other) {
        if (other.CompareTag(playerTag) && player != null) {
            player.SetInteractable(null);
        }
    }

    public abstract void Interact();
}
