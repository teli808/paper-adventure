using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class Falling : Command
{
    public override void Execute(OverworldController controller)
    {
        StopSoundInstance(controller);

        Vector2 movementInput = OverworldInputManager.Instance.MovementInput;
        Rigidbody myRigidbody = controller.MyRigidbody;
        float speed = controller.Speed;

        Vector3 direction = new Vector3(movementInput.x, 0f, movementInput.y);

        myRigidbody.MovePosition(controller.transform.position + (direction * speed * Time.fixedDeltaTime));
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
