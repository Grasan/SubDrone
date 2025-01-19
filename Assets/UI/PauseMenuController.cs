using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuController : MonoBehaviour {
    public VisualElement ui;

    public Button playBTN;
    public Button optionsBTN;
    public Button exitBTN;

    private void Awake() {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable() {
        playBTN = ui.Q<Button>("PlayButton");
        playBTN.clicked += OnPlayButtonCLicked;

        optionsBTN = ui.Q<Button>("OptionsButton");
        optionsBTN.clicked += OnOptionsButtonCLicked;
        
        exitBTN = ui.Q<Button>("ExitButton");
        exitBTN.clicked += OnExitButtonCLicked;
    }

    private void OnPlayButtonCLicked() {
        gameObject.SetActive(false);
    }

    private void OnOptionsButtonCLicked() {
        Debug.Log("Options"); 
    }

    private void OnExitButtonCLicked() {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
