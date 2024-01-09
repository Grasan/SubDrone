using UnityEngine;

public class TreasurePiece : Treasure {
    [SerializeField] private Mesh model;

    private void Awake() {
        GetComponent<MeshFilter>().mesh = model;
    }

    public override void Interact() {
        base.Interact();

        // Remove object from the scene
        Destroy(gameObject);
    }
}