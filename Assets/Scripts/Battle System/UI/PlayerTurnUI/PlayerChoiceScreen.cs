using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerChoiceScreen : MonoBehaviour
{
    [SerializeField] PlayerScreenButton[] buttons;
    [SerializeField] float waitBetweenPresses = 0.3f;

    int currentHoveredButtonIndex = 0;

    float timeSinceLastPress = Mathf.Infinity;

    private void Update()
    {
        timeSinceLastPress += Time.deltaTime;
    }

    public void ShowScreenDefault(bool isDefault)
    {
        if (isDefault)
        {
            currentHoveredButtonIndex = 0;
        }

        transform.localScale = Vector3.zero;

        gameObject.SetActive(true);

        AnimationOnActive();

        buttons[currentHoveredButtonIndex].HighlightButton();
    }

    public void HideScreen()
    {
        gameObject.SetActive(false);
    }

    public void Move(string direction)
    {
        if (timeSinceLastPress < waitBetweenPresses) return;

        timeSinceLastPress = 0f;

        buttons[currentHoveredButtonIndex].IdleButton();

        if (direction == "up")
        {
            if (currentHoveredButtonIndex - 1 < 0)
            {
                currentHoveredButtonIndex = buttons.Length - 1;
            }
            else
            {
                currentHoveredButtonIndex -= 1;
            }
        }
        else if (direction == "down")
        {
            if (currentHoveredButtonIndex + 1 == buttons.Length)
            {
                currentHoveredButtonIndex = 0;
            }
            else
            {
                currentHoveredButtonIndex += 1;
            }
        }

        buttons[currentHoveredButtonIndex].HighlightButton();
    }

    public bool CheckValidButton()
    {
        return buttons[currentHoveredButtonIndex].CheckValidButton();
    }

    public void PressHoveredButton()
    {
        buttons[currentHoveredButtonIndex].ButtonClicked();
    }

    public void AnimationOnActive()
    {
        transform.DOScale(Vector3.one, 0.2f);
    }
}
