using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : Command
{
    public override void Execute(OverworldController controller)
    {
        StopSoundInstance(controller);
    }

    public override void UpdateSound(OverworldController controller)
    {
        return;
    }

    public override void StopSoundInstance(OverworldController controller)
    {
        AudioManager.Instance.StopEventInstance(controller.LastSoundPlaying, STOP_MODE.ALLOWFADEOUT);
    }
}
