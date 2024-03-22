using FMOD.Studio;
using UnityEngine;

public class Move : Command
{
    EventInstance playerFootsteps;

    private void Start()
    {
        playerFootsteps = AudioManager.Instance.Create3DEventInstance(FMODEvents.Instance.Overworld.PlayerFootsteps, transform.position);
    }
    public override void Execute(OverworldController controller)
    {
        StopSoundInstance(controller);
        UpdateSound(controller);

        Vector2 movementInput = controller.MovementInput;
        Rigidbody myRigidbody = controller.MyRigidbody;
        float speed = controller.Speed;

        Vector3 direction = new Vector3(movementInput.x, 0f, movementInput.y);

        myRigidbody.MovePosition(controller.transform.position + (direction * speed * Time.fixedDeltaTime));
    }

    public override void UpdateSound(OverworldController controller)
    {
        AudioManager.Instance.UpdateEventInstanceAttributes(playerFootsteps, transform.position);

        controller.LastSoundPlaying = playerFootsteps;

        PLAYBACK_STATE playbackState;
        playerFootsteps.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            playerFootsteps.start();
        }
    }

    public override void StopSoundInstance(OverworldController controller)
    {
        if (!controller.LastSoundPlaying.Equals(playerFootsteps))
        {
            AudioManager.Instance.StopEventInstance(controller.LastSoundPlaying, STOP_MODE.ALLOWFADEOUT);
        }
    }
}
