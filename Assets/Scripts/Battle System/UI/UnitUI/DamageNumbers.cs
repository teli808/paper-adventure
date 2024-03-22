using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageNumbers : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI damageNumbersText;
    [SerializeField] float timeOnScreen = 0.9f;
    [SerializeField] float distanceNumberMoves = 15f;

    Vector3 startingPosition;

    private void Awake()
    {
        startingPosition = transform.position;
        ResetSettings();
    }

    public IEnumerator PlayDamageAnimation(int damageTaken)
    {
        ResetSettings();

        damageNumbersText.text = damageTaken.ToString();
        gameObject.SetActive(true);

        transform.DOScale(1f, 0.3f);

        transform.DOLocalMoveX(distanceNumberMoves, 0.7f);

        yield return new WaitForSeconds(timeOnScreen);

        transform.DOScale(0f, 0.2f).OnComplete(() => gameObject.SetActive(false));
    }

    private void ResetSettings()
    {
        gameObject.SetActive(false);
        transform.position = startingPosition;
        transform.localScale = Vector3.zero;
    }
}
