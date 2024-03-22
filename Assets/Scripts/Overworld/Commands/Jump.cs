using FMOD.Studio;
using UnityEngine;

public class Jump : Command
{
    public override void Execute(OverworldController controller)
    {
        StopSoundInstance(controller);
        UpdateSound(controller);

        Rigidbody myRigidbody = controller.MyRigidbody;
        float jumpForce = controller.JumpForce;

        myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public override void UpdateSound(OverworldController controller)
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Overworld.PlayerJump, transform.position);
    }

    public override void StopSoundInstance(OverworldController controller)
    {
        AudioManager.Instance.StopEventInstance(controller.LastSoundPlaying, STOP_MODE.ALLOWFADEOUT);
    }
}