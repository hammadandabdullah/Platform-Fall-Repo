using UnityEngine;
using Unity.Netcode;

public class CharacterJump : NetworkBehaviour
{
    Rigidbody rigidbody;
    public float jumpStrength = 2;
    public event System.Action Jumped;

    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    GroundCheck groundCheck;

    private bool jumpInputApplied = false;
    private bool isJumping = false;

    void Reset()
    {
        // Try to get groundCheck.
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    void Awake()
    {
        // Get rigidbody.
        rigidbody = GetComponent<Rigidbody>();

        if (GetComponent<BotDecisionMaker>())
        {
            jumpStrength /= 3;
        }
    }

    void Update()
    {
        if (!IsOwner) return;
        //if (!CharacterInitialPosition.canMove) return;

        // Jump when the Jump button is pressed and we are on the ground.

        if (jumpInputApplied && (!groundCheck || groundCheck.isGrounded))
        {
            rigidbody.AddForce(Vector3.up * 100 * jumpStrength);
            Jumped?.Invoke();
            isJumping = true;
            jumpInputApplied = false;
            Debug.Log("JUMP -- Jumping");
        }
        else
        {
            isJumping = false;
        }
    }

    public void ApplyJump()
    {
        if (!isJumping)
        {
            jumpInputApplied = true;
        }
    }
}
