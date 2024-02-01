using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    [SerializeField] private GameObject mainMenuCanvasGO;
    [SerializeField] private GameObject settingsMenuCanvasGO;

    private bool isPaused;

    void Start() {
        mainMenuCanvasGO.SetActive(false);
        settingsMenuCanvasGO.SetActive(false);
    }

    private void Update() {
        if (InputManager.Instance.MenuOpenCloseInput) {
            if (!isPaused)
                Pause();
            else
                Unpause();
        }
    }

    #region Pause/Unpause Functions

    public void Pause() { 
        isPaused = true;
        Time.timeScale = 0f;

        OpenMainMenu();
    }

    public void Unpause() {

    }

    #endregion
    #region Canvas Management

    public void OpenMainMenu() {
        mainMenuCanvasGO.SetActive(true);
        settingsMenuCanvasGO.SetActive(false);
    }

    #endregion
}
