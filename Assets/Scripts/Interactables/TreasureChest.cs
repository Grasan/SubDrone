using UnityEngine;

public class TreasureChest : Treasure {

    //Animator openingClip;

    private void Awake() {
        if (treasureSO.type == TreasureSO.TreasureType.Chest)
            Debug.LogWarning("You're trying to use a treasure not of chest type!");
    }

    public override void Interact() {
        base.Interact();
        // Animate The lid.

        // Audio effect.

        // Remove the trigger component.
        Destroy(_trigger);
    }
}