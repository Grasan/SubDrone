using UnityEngine;

namespace SubDrone.Interactables {
    [CreateAssetMenu(fileName = "newTreasurePiece", menuName = "Treasure/Treaure Piece")]
    public class TreasurePieceScriptableObject : BaseTreasureScriptableObject {
        public enum TreasureType { SmallPiece, MediumPiece, LargePiece, Quest };
        public TreasureType Type;
    }
}