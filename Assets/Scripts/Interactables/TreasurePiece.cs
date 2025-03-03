using UnityEngine;

namespace SubDrone.Interactables {
    public class TreasurePiece : Treasure<TreasurePieceScriptableObject> {
        
        public override void Interact() {
            _player.EarnPoints(GetPoints());
            if (treasure.obtainSound != null)
                AudioSource.PlayClipAtPoint(treasure.obtainSound, transform.position);
            Destroy(gameObject);
        }
    }
}
