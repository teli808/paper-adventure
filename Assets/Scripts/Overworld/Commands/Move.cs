using UnityEngine;

public class Move : Command
{
    public override void Execute(OverworldController controller)
    {
        Vector2 movementInput = OverworldInputManager.Instance.MovementInput;
        Rigidbody myRigidbody = controller.MyRigidbody;
        float speed = controller.Speed;

        Vector3 direction = new Vector3(movementInput.x, 0f, movementInput.y);

        myRigidbody.MovePosition(controller.transform.position + (direction * speed * Time.fixedDeltaTime));
    }
}
