using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject firstSelectedEventSystemObject;

    Canvas pauseMenuCanvas;

    private void Awake()
    {
        pauseMenuCanvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        DisableCanvas();
    }

    public void ShowMenu()
    {
        EnableCanvas();

        EventSystem.current.SetSelectedGameObject(firstSelectedEventSystemObject);

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        DisableCanvas();

        GameObject.FindWithTag("Player").GetComponent<PlayerOverworldController>().ResumeFromPauseMenu();

        Time.timeScale = 1f;
    }

    private void DisableCanvas()
    {
        pauseMenuCanvas.enabled = false;
        GetComponent<CanvasGroup>().interactable = false;
    }

    private void EnableCanvas()
    {
        pauseMenuCanvas.enabled = true;
        GetComponent<CanvasGroup>().interactable = true;
    }

    //Settings

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
