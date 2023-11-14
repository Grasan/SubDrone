using UnityEngine;

public class TreasureChest : Treasure {

    //Animator openingClip;

    private void Awake() {

    }

    public override void Interact() {
        base.Interact();

        // Animate The lid.


        // Remove the trigger component.
        Destroy(trigger);
    }
}