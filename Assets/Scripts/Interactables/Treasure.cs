using UnityEngine;

namespace SubDrone {
    public class Treasure : Interactable {
        public TreasureSO treasureSO;

        /* Components 
        private MeshFilter filter;
        private MeshRenderer renderer;

        private void Awake() {
            filter = GetComponent<MeshFilter>();
            renderer = GetComponent<MeshRenderer>();

            filter.mesh = treasureSO.model;
            renderer.material = treasureSO.material;
        }*/

        public override void Interact() {
            _player.EarnPoints(treasureSO.points);
        }

    }
}