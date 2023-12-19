using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldController : MonoBehaviour
{
    protected FlipCharacter flipCharacter;
    protected Command moveCommand;

    [field: SerializeField] public float Speed { get; private set; } = 4f;
    [field: SerializeField] public float JumpForce { get; private set; } = 6f;

    protected Animator animator;
    public Rigidbody MyRigidbody;
    protected CapsuleCollider capsuleCollider;

    //public Vector2 MovementInput { get; set; }

    public virtual void Awake()
    {
        moveCommand = GetComponent<Move>();
        flipCharacter = GetComponent<FlipCharacter>();

        animator = GetComponentInChildren<Animator>();
        MyRigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        //print("Overworld Controller called awake for: " + transform.name); //called everytime scene is loaded
    }

    public virtual void Start()
    {
        //print("Overworld Controller called start for: " + transform.name);
    }

    public bool IsCharacterMoving()
    {
        bool playerHasHorizontalSpeed = !Mathf.Approximately(OverworldInputManager.Instance.MovementInput.x, Mathf.Epsilon);
        bool playerHasForwardSpeed = !Mathf.Approximately(OverworldInputManager.Instance.MovementInput.y, Mathf.Epsilon);

        return playerHasHorizontalSpeed || playerHasForwardSpeed;
    }
}
