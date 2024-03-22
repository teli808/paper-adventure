using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    [SerializeField] protected Image statBarBackground;
    [SerializeField] protected TextMeshProUGUI currentStatComponent;
    [SerializeField] protected TextMeshProUGUI maxStatComponent;
    [SerializeField] protected Image icon;

    [SerializeField] float transitionTime = 0.2f;

    bool isInitialized = false;

    //Transform startingTransform;

    private void Awake()
    {
        //startingTransform = transform;
        transform.localScale = Vector3.zero;
        ShowAllChildObjects(false);
    }

    private void OnEnable()
    {
        EventManager.Instance.OnBattleSetUp += InitialSetup;
        EventManager.Instance.OnChangedBattleState += ToggleStatBar;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnBattleSetUp -= InitialSetup;
        EventManager.Instance.OnChangedBattleState -= ToggleStatBar;
    }

    private void Update()
    {
        if (!isInitialized) return;
        UpdateCurrentStat();
    }

    private void InitialSetup()
    {
        UpdateCurrentStat();
        UpdateMaxStat();
        ShowBar();
        isInitialized = true;
    }

    public virtual void UpdateCurrentStat() { }

    public virtual void UpdateMaxStat() { }

    private void ToggleStatBar(BattleState battleState)
    {
        if (battleState == BattleState.PLAYERUI || battleState == BattleState.ENEMYTURN)
        {
            ShowBar();
        }
        else if (battleState == BattleState.DIALOGUE || battleState == BattleState.LOST || battleState == BattleState.WON)
        {
            HideBar();
        }
    }

    private void HideBar()
    {
        transform.DOScale(Vector3.zero, transitionTime).OnComplete(() => ShowAllChildObjects(false));
        //transform.DOMoveY(0f, 0.2f);
    }

    private void ShowBar()
    {
        ShowAllChildObjects(true);
        transform.DOScale(Vector3.one, transitionTime * 2);
        //transform.DOMoveY(startingTransform.position.y, 0.2f);
    }

    private void ShowAllChildObjects(bool shouldShow)
    {
        statBarBackground.enabled = shouldShow;
        currentStatComponent.enabled = shouldShow;
        maxStatComponent.enabled = shouldShow;
        icon.enabled = shouldShow;
    }
}
