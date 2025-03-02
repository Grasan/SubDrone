using UnityEngine;

namespace SubDrone.Interactables {
    [CreateAssetMenu(fileName = "newTreasureChest", menuName = "Treasure/Treasure Chest")]
    public class TreasureChestScriptableObject : ScriptableObject {
        public new string name;
        public GameObject treasurePrefab;
        public int points;
        public AudioClip obtainSound;
    }
}
