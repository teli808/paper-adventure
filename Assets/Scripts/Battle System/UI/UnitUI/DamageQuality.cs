using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageQuality : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI damageQualityText;
    [SerializeField] float timeOnScreen = 0.5f;

    private void Awake()
    {
        ResetSettings();
    }

    public IEnumerator PlayDamageQualityAnimation(bool wasTimedAttack)
    {
        ResetSettings();

        if (wasTimedAttack)
        {
            damageQualityText.text = "GREAT";
        }
        else
        {
            damageQualityText.text = "OK";
        }

        gameObject.SetActive(true);

        transform.DOScale(1f, 0.2f);

        yield return new WaitForSeconds(timeOnScreen);

        damageQualityText.DOFade(0f, 0.1f).OnComplete(() => gameObject.SetActive(false));
    }

    private void ResetSettings()
    {
        gameObject.SetActive(false);
        damageQualityText.alpha = 1f;
        transform.localScale = Vector3.zero;
    }
}
