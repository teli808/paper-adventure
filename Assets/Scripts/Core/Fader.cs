using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    [SerializeField] float fadeTime = 0.5f;

    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        StartCoroutine(ShortFade(0f));
    }

    public IEnumerator ShortFade(float target)
    {
        while (!Mathf.Approximately(canvasGroup.alpha, target))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / fadeTime);
            yield return null;
        }

        if (target == 0f) FadeComplete(); //once the fader goes from white to completely transparent
    }

    public IEnumerator LongFade(float target)
    {
        while (!Mathf.Approximately(canvasGroup.alpha, target))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / fadeTime * 2);
            yield return null;
        }

        if (target == 0f) FadeComplete();
    }

    private void FadeComplete()
    {
        EventManager.Instance.FaderComplete();
        print("FadeComplete");
    }
}
