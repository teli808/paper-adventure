using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEventsBattle : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference DefaultBattleBGM { get; private set; }
    [field: SerializeField] public EventReference WizardBossBGM { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference PlayerFootsteps { get; private set; }
    [field: SerializeField] public EventReference PlayerJump { get; private set; }
    [field: SerializeField] public EventReference TimedCorrectly { get; private set; }
    [field: SerializeField] public EventReference BlockedCorrectly { get; private set; }

    [field: Header("UI SFX")]
    [field: SerializeField] public EventReference NavigateUI { get; private set; }
    [field: SerializeField] public EventReference AcceptButton { get; private set; }
    [field: SerializeField] public EventReference BackButton { get; private set; }


}
