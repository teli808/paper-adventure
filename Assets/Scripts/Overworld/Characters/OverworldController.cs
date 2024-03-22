using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OverworldController : MonoBehaviour
{
    protected FlipCharacter flipCharacter;
    protected Command moveCommand;

    [field: SerializeField] public float Speed { get; private set; } = 4f;
    [field: SerializeField] public float JumpForce { get; private set; } = 6f;
    public EventInstance LastSoundPlaying { get; set; }
    public Vector2 MovementInput { get; set; } //required for Move Command

    public Rigidbody MyRigidbody;
    protected CapsuleCollider capsuleCollider;

    public virtual void Awake()
    {
        moveCommand = GetComponent<Move>();
        flipCharacter = GetComponent<FlipCharacter>();

        MyRigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        MyRigidbody.Sleep(); //needed to prevent transform.position issues after loading into a scene
    }

    public abstract bool CheckMoveInput();
}
