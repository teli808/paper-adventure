using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagementDisplay : MonoBehaviour
{
    [SerializeField] InventoryScreen inventoryScreen;
    [SerializeField] StatsScreen statsScreen;

    [SerializeField] PlayerManagementTopBar topBar; //handles moving between screens and maintaining color state

    Canvas playerManagementCanvas;

    private void Awake()
    {
        playerManagementCanvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        DisableCanvas();
    }

    public void OpenInventoryDisplay() //based off button press in PlayerController, a screen within PlayerManagementDisplay will open
    {
        Time.timeScale = 0f;

        EnableCanvas();

        topBar.ShowBar();
        inventoryScreen.ShowScreen();

        //....
    }

    public void OpenStatsDisplay()
    {
        Time.timeScale = 0f;

        EnableCanvas();

        topBar.ShowBar();
        statsScreen.ShowScreen();
    }

    public void ClosePlayerManagementDisplay()
    {
        Time.timeScale = 1f;

        DisableCanvas();

        topBar.HideBar();

        inventoryScreen.HideScreen();
        statsScreen.HideScreen();
    }

    private void DisableCanvas()
    {
        playerManagementCanvas.enabled = false;
        GetComponent<CanvasGroup>().interactable = false;
    }

    private void EnableCanvas()
    {
        playerManagementCanvas.enabled = true;
        GetComponent<CanvasGroup>().interactable = true;
    }
}
