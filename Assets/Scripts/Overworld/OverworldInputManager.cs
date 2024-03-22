using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class OverworldInputManager : MonoBehaviour
{
    public static OverworldInputManager Instance { get; private set; }

    PlayerInput playerInput;
    public Vector2 MovementInput { get; private set; }

    public bool JumpInput { get; set; } = false;

    private bool interactInput = false;

    private bool pauseMenuInput  = false;

    private bool inventoryMenuInput = false;

    private bool statsMenuInput = false;

    //UI

    private bool cancelPlayerManagementInput = false;

    private void Awake()
    {
        if (Instance != null)
        {
            print("Multiple input managers exist");
        }

        Instance = this;

        playerInput = GetComponent<PlayerInput>();
    }

    //Gameplay Action Map
    public void DisableGameplayActionMap()
    {
        playerInput.actions.FindActionMap("Gameplay").Disable();
    }

    public void EnableGameplayActionMap()
    {
        playerInput.actions.FindActionMap("Gameplay").Enable();
    }

    public void SwitchToGameplayActionMap()
    {
        playerInput.currentActionMap = playerInput.actions.FindActionMap("Gameplay");
    }

    public void SwitchToInMenuMap()
    {
        playerInput.currentActionMap = playerInput.actions.FindActionMap("InMenu");
    }

    public void Move(InputAction.CallbackContext value)
    {
        MovementInput = value.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext value)
    {
        if (value.action.IsPressed())
        {
            if (value.canceled) return;

            JumpInput = true;
        }
    }

    public void DisableJump()
    {
        playerInput.actions.FindAction("Jump").Disable();
    }

    public void EnableJump()
    {
        playerInput.actions.FindAction("Jump").Enable();
    }

    public void Interact(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            interactInput = true;
        }
        else if (value.canceled)
        {
            interactInput = false;
        }

    }

    public bool GetInteractInput()
    {
        bool initialState = interactInput;
        interactInput = false;
        return initialState;
    }

    public void PauseMenu(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            pauseMenuInput = true;
        }
        else if (value.canceled)
        {
            pauseMenuInput = false;
        }
    }

    public bool GetPauseMenuInput()
    {
        bool initialState = pauseMenuInput;
        pauseMenuInput = false;
        return initialState;
    }

    public void InventoryMenu(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            inventoryMenuInput = true;
        }
        else if (value.canceled)
        {
            inventoryMenuInput = false;
        }
    }

    public bool GetInventoryMenuInput()
    {
        bool initialState = inventoryMenuInput;
        inventoryMenuInput = false;
        return initialState;
    }

    public void StatsMenu(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            statsMenuInput = true;
        }
        else if (value.canceled)
        {
            statsMenuInput = false;
        }
    }

    public bool GetStatsMenuInput()
    {
        bool initialState = statsMenuInput;
        statsMenuInput = false;
        return initialState;
    }

    //In Menu Action Map

    public void CancelPlayerManagementScreen(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            cancelPlayerManagementInput = true;
        }
        else if (value.canceled)
        {
            cancelPlayerManagementInput = false;
        }
    }

    public bool GetCancelPlayerManagementInput()
    {
        bool initialState = cancelPlayerManagementInput;
        cancelPlayerManagementInput = false;
        return initialState;
    }
}
