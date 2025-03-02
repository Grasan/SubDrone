using UnityEngine;

namespace SubDrone.Interactables {
    [CreateAssetMenu(fileName = "newTreasurePiece", menuName = "Treasure/Treaure Piece")]
    public class TreasurePieceScriptableObject : ScriptableObject {
        public new string name;
        public GameObject treasurePrefab;
        public TreasureType type;
        public int points;
        public AudioClip pickupSound;

        public enum TreasureType { SmallPiece, MediumPiece, LargePiece, Quest };
    }
}