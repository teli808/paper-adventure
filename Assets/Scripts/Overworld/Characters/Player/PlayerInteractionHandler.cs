using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionHandler : MonoBehaviour
{
    public bool IsTouchingInteractable { get; private set; } = false;

    public IInteractable CurrentInteractableObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null)
        {
            IsTouchingInteractable = true;
            CurrentInteractableObject = other.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null)
        {
            IsTouchingInteractable = false;
            CurrentInteractableObject = null;
        }
    }
}
