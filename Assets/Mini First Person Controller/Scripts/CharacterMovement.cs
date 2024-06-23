using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterMovement : NetworkBehaviour
{
    public Animator anim;
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;

    [Space]
    [Header("Push")]
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private float getPushedForce = 10f;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    private float inputX;
    private float inputY;

    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    GroundCheck groundCheck;


    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;
        if (!GameManager.canMove) return;
        bool notGroundedForSomeTime = IsNotGroundedForTime(2);
        if (notGroundedForSomeTime) return;

        // Update IsRunning from input.
        //IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity = new Vector2( inputX * targetMovingSpeed, inputY * targetMovingSpeed);

        // Apply movement.
        rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);

        Vector3 movementDirection = targetVelocity.normalized;
        float blend = movementDirection.magnitude;
        anim.SetFloat("Blend", blend);
    }

    float notGroundedTimer = 0;
    private bool IsNotGroundedForTime(float totalNotGroundedTime)
    {
        if (!groundCheck.isGrounded)
        {
            notGroundedTimer += Time.deltaTime;

            if(notGroundedTimer >= totalNotGroundedTime)
            {
                return true;
            }
        }
        else
        {
            notGroundedTimer = 0;
        }

        return false;
    }

    public void SetInputX(float givenX)
    {
        inputX = givenX;
    }

    public void SetInputY(float givenY)
    {
        inputY = givenY;
    }

    public void ApplyPushForce()
    {
        rigidbody.AddForce(transform.forward * pushForce * Time.deltaTime, ForceMode.Impulse);
    }

    public void GetPushForce(Vector3 otherPlayerForward)
    {
        rigidbody.AddForce(otherPlayerForward * getPushedForce * Time.deltaTime, ForceMode.Impulse);
    }
}