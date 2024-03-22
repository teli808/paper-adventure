using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScreenButton : MonoBehaviour
{
    [SerializeField] Color idleColor;
    [SerializeField] Color hoveredColor;

    Image buttonBackground;

    private void Awake()
    {
        buttonBackground = GetComponent<Image>();

        idleColor = new Color(1, 1, 1, 0.34f);
        hoveredColor = new Color(0.97f, 1, 0.5f, 0.58f);
    }

    private void OnEnable()
    {
        buttonBackground.color = idleColor;
    }

    public void IdleButton()
    {
        buttonBackground.color = idleColor;
    }

    public void HighlightButton()
    {
        buttonBackground.color = hoveredColor;
    }

    public virtual void ButtonClicked() { }

    public virtual bool CheckValidButton()
    {
        return true;
    }
}
