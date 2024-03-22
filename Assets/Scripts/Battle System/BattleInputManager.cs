using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleInputManager : MonoBehaviour
{
    public static BattleInputManager Instance { get; private set; }

    private bool mainKeyPressed = false; //interact & attack
    private bool blockKeyPressed = false;
    private Vector2 navigateKeyInput; //arrow keys for menu

    private void Awake()
    {
        if (Instance != null)
        {
            print("Found more than one Battle Input Manager in the scene");
        }

        Instance = this;
    }

    public void OnMainKeyPress(InputAction.CallbackContext value) //currently F
    {
        if (value.performed)
        {
            mainKeyPressed = true;
        }
        else if (value.canceled)
        {
            mainKeyPressed = false;
        }
    }

    public void OnBlockKeyPress(InputAction.CallbackContext value) //currently B
    {
        if (value.performed)
        {
            blockKeyPressed = true;
        }
        else if (value.canceled)
        {
            blockKeyPressed = false;
        }
    }

    public void OnNavigateKeyPress(InputAction.CallbackContext value) //arrow keys
    {
        if (value.performed)
        {
            navigateKeyInput = value.ReadValue<Vector2>();
        }
        else if (value.canceled)
        {
            navigateKeyInput = value.ReadValue<Vector2>();
        }
    }


    public bool GetMainKeyPressed()
    {
        bool initialState = mainKeyPressed;
        mainKeyPressed = false;
        return initialState;
    }

    public bool GetBlockKeyPressed()
    {
        bool initialState = blockKeyPressed;
        blockKeyPressed = false;
        return initialState;
    }

    public Vector2 GetNavigateKeyInput()
    {
        // Vector2 initialInput = navigateKeyInput;
        // navigateKeyInput = Vector2.zero;
        // return initialInput;

        return navigateKeyInput;
    }
}
