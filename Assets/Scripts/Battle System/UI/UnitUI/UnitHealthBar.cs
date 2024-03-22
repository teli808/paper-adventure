using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UnitHealthBar : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] Image bar;
    [SerializeField] Image movingHealth;

    [SerializeField] float animationTime = 0.2f;

    bool attackedBefore = false;

    private void OnEnable()
    {
        EventManager.Instance.OnChangedBattleState += ToggleHealthBar;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnChangedBattleState -= ToggleHealthBar;
    }

    private void Awake()
    {
        HideComponents();
    }

    public void SetAttackedBefore()
    {
        attackedBefore = true;
    }

    public void PlayHealthAnimation(float newHP, float previousHP, float maxHP)
    {
        float startingScale = previousHP / maxHP;
        movingHealth.transform.localScale = new Vector3(startingScale, movingHealth.transform.localScale.y, movingHealth.transform.localScale.z);

        ShowComponents();

        float newScale = newHP / maxHP;

        movingHealth.transform.DOScaleX(newScale, animationTime);
    }

    private void ToggleHealthBar(BattleState battleState)
    {
        if (!attackedBefore) return;

        if (battleState == BattleState.PLAYERUI)
        {
            ShowComponents();
        }
        else if (battleState == BattleState.PLAYERATTACK)
        {
            HideComponents();
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            HideComponents();
        }
    }

    private void HideComponents()
    {
        background.enabled = false;
        bar.enabled = false;
        movingHealth.enabled = false;
    }

    private void ShowComponents()
    {
        background.enabled = true;
        bar.enabled = true;
        movingHealth.enabled = true;
    }
}
