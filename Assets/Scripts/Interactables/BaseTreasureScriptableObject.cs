using UnityEngine;

namespace SubDrone.Interactables { 
    public class BaseTreasureScriptableObject : ScriptableObject {
        public string treasureName;
        public int points;
        public AudioClip obtainSound;
        public GameObject prefab;
    }
}
