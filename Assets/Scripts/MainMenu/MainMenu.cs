using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Buttons")]

    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;

    private void Start()
    {
        if (!DataPersistenceManager.Instance.CheckGameFileExists())
        {
            continueGameButton.interactable = false;
        }
    }

    public void OnNewGameClicked()
    {
        DisableMenuButtons();

        DataPersistenceManager.Instance.NewGame();

        //SceneManager.LoadSceneAsync("Tutorial");
    }

    public void OnContinueGameClicked()
    {
        DisableMenuButtons();

        DataPersistenceManager.Instance.ContinueGame();

        //SceneManager.LoadSceneAsync("Tutorial");
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }
}
