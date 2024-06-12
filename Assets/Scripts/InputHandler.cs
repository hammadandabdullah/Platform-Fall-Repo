using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class InputHandler : NetworkBehaviour
{
    [SerializeField] private bool isPlayer;

    private CharacterJump jumpScript;
    private CharacterMovement movementScript;
    private PlayerRotation playerRotationScript;

    private void Start()
    {
        jumpScript = GetComponent<CharacterJump>();
        movementScript = GetComponent<CharacterMovement>();

        if (isPlayer)
        {
            playerRotationScript = GetComponentInChildren<PlayerRotation>();
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        if (isPlayer)
        {
            movementScript.SetInputX(Input.GetAxis("Horizontal"));
            movementScript.SetInputY(Input.GetAxis("Vertical"));

            playerRotationScript.SetInputX(Input.GetAxis("Mouse X"));
            playerRotationScript.SetInputY(Input.GetAxis("Mouse Y"));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpScript.ApplyJump();
            }
        }
    }

    public void SetBotMovement(bool canGoForward)
    {
        movementScript.SetInputY(canGoForward ? 1 : 0);
    }

    public void SetBotJump(bool canJump)
    {
        if (canJump)
        {
            jumpScript.ApplyJump();
        }
    }
}
