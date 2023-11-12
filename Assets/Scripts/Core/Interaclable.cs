using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interaclable : MonoBehaviour {

    protected SphereCollider trigger;
    protected Drone player;

    private void Awake() {
        trigger = GetComponent<SphereCollider>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            player = (Drone)other.GetComponent<Drone>();
            player.SetInteractable(this);
        }
    }

    public abstract void Interact();
}
