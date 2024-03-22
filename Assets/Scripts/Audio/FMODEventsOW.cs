using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEventsOW : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference TutorialBGM { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference PlayerFootsteps { get; private set; } //also used for enemies
    [field: SerializeField] public EventReference PlayerJump { get; private set; }

    [field: Header("Enemy SFX")]
    [field: SerializeField] public EventReference EnemyChaseVisualSound { get; private set; }

    [field: Header("SaveBlock SFX")]
    [field: SerializeField] public EventReference SaveBlockHit { get; private set; }

    [field: Header("Wizard SFX")]
    [field: SerializeField] public EventReference WizardAura { get; private set; } //Use FMod Studio Event Emitter component for audio by distance
}
