using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseVisualCue : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    public IEnumerator DisplayCue()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Overworld.EnemyChaseVisualSound, transform.position);
        spriteRenderer.enabled = true;

        yield return new WaitForSeconds(1.5f);

        spriteRenderer.enabled = false;
    }
}
