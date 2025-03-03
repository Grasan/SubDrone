using UnityEngine;

namespace SubDrone.Interactables {
    [CreateAssetMenu(fileName = "newTreasureChest", menuName = "Treasure/Treasure Chest")]
    public class TreasureChestScriptableObject : BaseTreasureScriptableObject {
        public GameObject lidPrefab;
    }
}
