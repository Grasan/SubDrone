using UnityEngine;

[CreateAssetMenu(fileName = "newTreasure", menuName = "Treasure")]
public class TreasureSO : ScriptableObject {
    public new string name;
    public GameObject treasurePrefab;
    public TreasureType type;
    public int points;
    public bool isQuestItem;
    public AudioClip pickupSound;

    public enum TreasureType { SmallPiece, MediumPiece, LargePiece, Chest, Quest };
}