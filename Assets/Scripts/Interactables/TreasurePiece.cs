using UnityEngine;

public class TreasurePiece : Treasure {
    [SerializeField] private Mesh model;

    private void Awake() {
        GetComponent<MeshFilter>().mesh = model;
    }

    public override void Interact() {
        base.Interact();

        Destroy(gameObject);
    }
}