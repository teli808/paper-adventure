using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerManagementScreen : MonoBehaviour
{
    [SerializeField] protected PlayerManagementTopBar topBar; //handles moving between screens and maintaining color state

    protected bool isEnabled = false;

    public virtual void Update()
    {
        if (!isEnabled) return;
    }

    public virtual void ShowScreen()
    {
        gameObject.SetActive(true);
        isEnabled = true;
    }
    public void HideScreen()
    {
        gameObject.SetActive(false);

        isEnabled = false;
    }
}
