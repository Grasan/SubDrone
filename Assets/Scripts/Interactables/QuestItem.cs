using UnityEngine;

public class QuestItem : Treasure {
    

    public override void Interact() {
        base.Interact();

        // Special audio effect.

        // message player, mission complete.

        // Remove ocject from scene.
        Destroy(gameObject);
    }
}
