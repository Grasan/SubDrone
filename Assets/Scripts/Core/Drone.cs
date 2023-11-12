using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour {

    private float health = 1;
    private int score = 0;

    private Interaclable interactable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(/*Interaclable interaclable*/) {
        if ( interactable != null )
            interactable.Interact( );
    }

    public void SetInteractable(Interaclable interaclable) {
        this.interactable = interaclable;
    }

    public void EarnPoints(int points) {
        score += points;
    }
}
