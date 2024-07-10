using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    public Canvas menu;

    private void Awake() {
        menu.enabled = false;
    }

    public void Pause() {
        menu.enabled = true; 
    }
    public void UnPause() {
        menu.enabled = false;
    }
}
