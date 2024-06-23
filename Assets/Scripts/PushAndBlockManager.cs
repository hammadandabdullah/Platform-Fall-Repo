using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PushAndBlockManager : NetworkBehaviour
{
    public enum State
    {
        Idle,
        Block,
        Push
    }

    [SerializeField] private SkinnedMeshRenderer headRenderer;

    private CharacterMovement movementScript;

    private State currentState = State.Idle;
    [SerializeField] private float pushTime = 1.5f;

    private bool pushInput = false;
    private bool blockInput = false;

    private void Start()
    {
        movementScript = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (pushInput || currentState == State.Push)
        {
            SetState(State.Push);
            pushInput = false;
        }
        else if (blockInput)
        {
            SetState(State.Block);
        }
        else if(currentState != State.Push)
        {
            SetState(State.Idle);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PushAndBlockManager>() != null)
            {
                PushAndBlockManager otherPlayerPushAndBlock = collision.gameObject.GetComponent<PushAndBlockManager>();

                bool isOtherPlayerPushing = otherPlayerPushAndBlock.GetCurrentState() == State.Push;
                bool thisPlayerCanGetPushed = currentState != State.Block;

                if (isOtherPlayerPushing && thisPlayerCanGetPushed)
                {
                    Vector3 otherPlayerForward = collision.transform.forward;
                    movementScript.GetPushForce(otherPlayerForward);
                    GetPushForceForOthersRpc(otherPlayerForward);
                }
            }
        }
    }

    public void SetState(State givenState)
    {
        SetStateRpc((int)givenState);
    }

    [Rpc(SendTo.Everyone)]
    private void SetStateRpc(int state)
    {
        switch (state)
        {
            case 0: 
                currentState = State.Idle;
                headRenderer.material.color = Color.white;
                break;
            case 1: 
                currentState = State.Block;
                headRenderer.material.color = Color.black;
                break;
            case 2: 
                currentState = State.Push;
                movementScript.ApplyPushForce();
                headRenderer.material.color = Color.green;
                Invoke(nameof(SetIdleState), pushTime);
                break;
            default:
                break;
        }
    }

    [Rpc(SendTo.NotMe)]
    private void GetPushForceForOthersRpc(Vector3 otherPlayerForward)
    {
        movementScript.GetPushForce(otherPlayerForward);
    }

    private void SetIdleState()
    {
        SetState(State.Idle);
    }

    public State GetCurrentState()
    {
        return currentState;
    }

    public void SetPushInput(bool givenInput)
    {
        if (currentState != State.Push)
            pushInput = givenInput;
    }

    public void SetBlockInput(bool givenInput)
    {
        blockInput = givenInput;
    }
}
