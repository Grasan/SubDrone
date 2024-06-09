using UnityEngine;

[CreateAssetMenu(fileName = "newTreasure", menuName = "Treasure")]
public class TreasureSO : ScriptableObject {
    public new string name;
    public int points;
    public Mesh model;
    public Material material;

    public TreasureType type;
    public enum TreasureType { SmallPiece, MediumPiece, LargePiece, Chest, Quest };
}