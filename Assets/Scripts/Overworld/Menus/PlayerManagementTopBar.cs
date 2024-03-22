using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerManagementTopBar : MonoBehaviour
{
    [SerializeField] private GameObject statsHeader;
    [SerializeField] private GameObject inventoryHeader;

    [SerializeField] private StatsScreen statsScreen;
    [SerializeField] private InventoryScreen inventoryScreen;

    private List<GameObject> allButtonObjects;
    GameObject currentTopBarVisual;
    //current GameObjectVisual selected

    private void Awake()
    {
        allButtonObjects = new List<GameObject>
        {
            statsHeader,
            inventoryHeader
        };
    }
    private void Start()
    {
        HideBar();
    }

    public void InventoryScreenOpened()
    {
        GameObject topBarVisual = inventoryHeader.GetComponentInChildren<SelectedTopBarVisual>(true).gameObject;

        SetCurrentSelectedVisual(topBarVisual);
    }

    public void StatsScreenOpened()
    {
        GameObject topBarVisual = statsHeader.GetComponentInChildren<SelectedTopBarVisual>(true).gameObject;

        SetCurrentSelectedVisual(topBarVisual);

        EventSystem.current.SetSelectedGameObject(statsHeader.gameObject);
    }

    public void InventoryButtonOnClick()
    {
        GameObject topBarVisual = inventoryHeader.GetComponentInChildren<SelectedTopBarVisual>(true).gameObject;

        ResetCurrentSelectedVisual();
        SetCurrentSelectedVisual(topBarVisual);

        inventoryScreen.ShowScreen();
        statsScreen.HideScreen();
    }

    public void StatsButtonOnClick()
    {
        GameObject topBarVisual = statsHeader.GetComponentInChildren<SelectedTopBarVisual>(true).gameObject;

        ResetCurrentSelectedVisual();
        SetCurrentSelectedVisual(topBarVisual);

        statsScreen.ShowScreen();
        inventoryScreen.HideScreen();
    }

    public bool CheckAnyBarButtonSelected()
    {
        return allButtonObjects.Contains(EventSystem.current.currentSelectedGameObject);
    }

    private void ResetCurrentSelectedVisual()
    {
        if (currentTopBarVisual != null)
        {
            currentTopBarVisual.SetActive(false);
        }

        currentTopBarVisual = null;
    }

    private void SetCurrentSelectedVisual(GameObject topBarVisual)
    {
        currentTopBarVisual = topBarVisual;
        currentTopBarVisual.SetActive(true);
    }

    public void ShowBar()
    {
        gameObject.SetActive(true);

        ResetCurrentSelectedVisual();
    }
    public void HideBar()
    {
        gameObject.SetActive(false);

        ResetCurrentSelectedVisual();
    }
}
