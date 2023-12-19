using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCharacter : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    public void HandleFlip(Vector2 movementInput)
    {
        if (movementInput.x > 0)
        {
            FlipRight();
        }
        else if (movementInput.x < 0)
        {
            FlipLeft();
        }
    }

    private void FlipRight()
    {
        spriteRenderer.flipX = false;
    }

    private void FlipLeft()
    {
        spriteRenderer.flipX = true;
    }
}
