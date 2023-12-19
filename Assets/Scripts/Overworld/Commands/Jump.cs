using UnityEngine;

public class Jump : Command
{
    public override void Execute(OverworldController controller)
    {
        Rigidbody myRigidbody = controller.MyRigidbody;
        float jumpForce = controller.JumpForce;

        myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}