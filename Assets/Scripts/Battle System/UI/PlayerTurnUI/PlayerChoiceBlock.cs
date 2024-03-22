using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerChoiceBlock : MonoBehaviour
{
    bool isHovered = false;

    [SerializeField] PlayerChoiceScreen playerChoiceScreen;

    TextMeshProUGUI buttonText;

    private void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.enabled = false;
    }

    public void SetHovered(bool isHovered)
    {
        this.isHovered = isHovered;

        if (isHovered)
        {
            buttonText.enabled = true;
        }
        else
        {
            buttonText.enabled = false;
        }
    }

    public PlayerChoiceScreen GetPlayerChoiceScreen()
    {
        return playerChoiceScreen;
    }
}
