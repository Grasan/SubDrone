using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {

    public static InputManager Instance;

    public bool MenuOpenCloseInput {  get; private set; }

    private PlayerInput _playerInput;

    private InputAction _menuOpenCloseAction;

    // Start is called before the first frame update
    void Awake() {
        if (Instance == null) {
            Instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();
        _menuOpenCloseAction = _playerInput.actions["MenuOpenClose"];
    }

    private void Update() {
        MenuOpenCloseInput = _menuOpenCloseAction.WasPressedThisFrame();
    }
}
