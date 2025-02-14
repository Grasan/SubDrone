using System.Collections.Generic;
using UnityEngine;

namespace SubDrone {
    public class Radar : MonoBehaviour {

        [Header("Radar settings")]
        public float radarRadius = 10f;
        public float scanInterval = 2.5f;
        private float _scanTimer;
        public LayerMask obstacleMask;

        private readonly List<Treasure> _nearbyTreasure = new List<Treasure>();

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
            if (other.TryGetComponent<Treasure>(out Treasure treasure)) {
                _nearbyTreasure.Add(treasure);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.TryGetComponent<Treasure>(out Treasure treasure)) {
                _nearbyTreasure.Remove(treasure);
            }
        }

        private void PingRadar() {
            Debug.Log("Radar ping sent");

            List<TreasurePing> radarPings = new List<TreasurePing>();

            foreach (Treasure treasure in _nearbyTreasure) {
                if (Vector3.Distance(transform.position, treasure.transform.position) <= radarRadius) {
                    bool isVisible = CheckVisibility(treasure);

                    radarPings.Add(new TreasurePing(treasure, isVisible));
                }
            }

            if (radarPings.Count > 0)
                Debug.Log($"{radarPings.Count} treasure(s) found!");
            else
                Debug.Log("Nothing found.");

            // UI functionality here.
        }

        private bool CheckVisibility(Treasure treasure) {
            Vector3 direction = (treasure.transform.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, treasure.transform.position);

            if (Physics.Raycast(transform.position, direction, distance, obstacleMask))
                return false;   // Blocked

            return true;    // Direct sight
        }
    }

    public class TreasurePing {
        public Treasure Treasure;
        public bool IsVisible;

        public TreasurePing(Treasure treasure, bool isVisible) {
            Treasure = treasure;
            IsVisible = isVisible;
        }
    }
}
