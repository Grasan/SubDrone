using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interaclable : MonoBehaviour {

    protected SphereCollider trigger;
    protected Drone player;

    private void Awake() {
        trigger = GetComponent<SphereCollider>();
        player = FindObjectOfType<Drone>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            player = GetComponent<Drone>();
            player.SetInteractable(this);
        }
    }

    public abstract void Interact();
}
