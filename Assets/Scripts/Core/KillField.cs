using UnityEngine;

public class KillField : MonoBehaviour {

    private GameObject player;

    public Transform resetPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {

            player.GetComponent<FirstPersonCharacter>().ResetPlayer(resetPosition);
        }
    }
}
