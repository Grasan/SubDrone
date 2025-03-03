using System.Collections.Generic;
using SubDrone.Interactables;
using UnityEngine;

namespace SubDrone.Core {
    public class Radar : MonoBehaviour {

        [Header("Radar settings")]
        public float radarRadius = 10f;
        public float scanInterval = 2.5f;
        private float _scanTimer;
        public LayerMask obstacleMask;

        private readonly List<TreasureBase> _nearbyTreasure = new List<TreasureBase>();

        private SphereCollider _trigger;

        private void Start() {
            _scanTimer = scanInterval;
            _trigger = GetComponent<SphereCollider>();
            _trigger.radius = radarRadius;
        }

        private void Update() {
            _scanTimer -= Time.deltaTime;
            if (_scanTimer <= 0) {
                PingRadar();
                _scanTimer = scanInterval;
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent<TreasureBase>(out var treasure)) {
                _nearbyTreasure.Add(treasure);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.TryGetComponent<TreasureBase>(out var treasure)) {
                _nearbyTreasure.Remove(treasure);
            }
        }

        private void PingRadar() {
            Debug.Log("Radar ping sent");

            var radarPings = new List<TreasurePing>();

            foreach (var treasure in _nearbyTreasure)
            {
                if (!(Vector3.Distance(transform.position, treasure.transform.position) <= radarRadius)) continue;
                
                var isVisible = CheckVisibility(treasure);
                radarPings.Add(new TreasurePing(treasure, isVisible));
            }

            Debug.Log(radarPings.Count > 0 ? $"{radarPings.Count} treasure(s) found!" : "Nothing found.");

            // UI functionality here.
        }

        private bool CheckVisibility(TreasureBase treasure) {
            var direction = (treasure.transform.position - transform.position).normalized;
            var distance = Vector3.Distance(transform.position, treasure.transform.position); 
            
            return !Physics.Raycast(transform.position, direction, distance, obstacleMask);
        }
    }

    public class TreasurePing {
        public TreasureBase Treasure;
        public bool IsVisible;

        public TreasurePing(TreasureBase treasure, bool isVisible) {
            Treasure = treasure;
            IsVisible = isVisible;
        }
    }
}
