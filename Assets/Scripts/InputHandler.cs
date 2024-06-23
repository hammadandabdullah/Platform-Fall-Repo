using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class InputHandler : NetworkBehaviour
{
    [SerializeField] private bool isPlayer;

    private CharacterJump jumpScript;
    private CharacterMovement movementScript;
    private CameraController playerRotationScript;
    private PushAndBlockManager pushAndBlockScript;

    private void Start()
    {
        jumpScript = GetComponent<CharacterJump>();
        movementScript = GetComponent<CharacterMovement>();
        pushAndBlockScript = GetComponent<PushAndBlockManager>();

        if (isPlayer)
        {
            playerRotationScript = GetComponentInChildren<CameraController>();
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        if (isPlayer)
        {
            //Movement
            movementScript.SetInputX(Input.GetAxis("Horizontal"));
            movementScript.SetInputY(Input.GetAxis("Vertical"));

            playerRotationScript.SetInputX(Input.GetAxis("Mouse X"));
            playerRotationScript.SetInputY(Input.GetAxis("Mouse Y"));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpScript.ApplyJump();
            }

            //Push And Block
            pushAndBlockScript.SetPushInput(Input.GetMouseButtonDown(0));
            pushAndBlockScript.SetBlockInput(Input.GetMouseButton(1));
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

    public void SetBotPush(bool canPush)
    {
        pushAndBlockScript.SetPushInput(canPush);
    }

    public void SetBotBlock(bool canBlock)
    {
        pushAndBlockScript.SetBlockInput(canBlock);
    }
}
