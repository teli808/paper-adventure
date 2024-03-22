using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    GameObject gameObject { get; }
    public void HandleUserInteraction() { }
}
