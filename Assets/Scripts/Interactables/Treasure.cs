using UnityEngine;

public class Treasure:Interaclable {

    [SerializeField] private int points;
    [SerializeField] private AudioClip pickupSound;

    public override void Interact() {
        player.EarnPoints(points);  // Give points to player

        // Play pickup sound

    }
}