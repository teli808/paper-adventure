using UnityEngine;

public abstract class Command : MonoBehaviour
{
    public abstract void Execute(OverworldController controller);

    public abstract void UpdateSound(OverworldController controller);

    public abstract void StopSoundInstance(OverworldController controller);
    //if sound instance in controller is different from what is currently playing, stop it
}