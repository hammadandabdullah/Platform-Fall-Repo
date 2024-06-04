using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class VibratingPlatform : NetworkBehaviour
{
    [SerializeField] private bool canVibrate = true;
    private float vibrationTime = 5f;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && canVibrate)
        {
            // Client -> Server because PingRpc sends to Server
            StartVibratingRpc(true);
        }
    }

    [Rpc(SendTo.Everyone)]
    private void StartVibratingRpc(bool canVibrate)
    {
        anim.SetBool("vibration", canVibrate);
        Invoke(nameof(TurnOffPlatform), vibrationTime);
    }

/*    [Rpc(SendTo.Server)]
    private void StartVibratingRpc(bool canVibrate)
    {
        StartVibratingClientRpc(canVibrate);
    }

    [Rpc(SendTo.NotServer)]
    private void StartVibratingClientRpc(bool canVibrate)
    {
        StartVibrating(canVibrate);
    }*/

    private void TurnOffPlatform()
    {
        gameObject.SetActive(false);
    }

    [Rpc(SendTo.Server)]
    public void PingRpc(int pingCount)
    {
        // Server -> Clients because PongRpc sends to NotServer
        // Note: This will send to all clients.
        // Sending to the specific client that requested the pong will be discussed in the next section.
        PongRpc(pingCount, "PONG!");
    }

    [Rpc(SendTo.NotServer)]
    void PongRpc(int pingCount, string message)
    {
        Debug.Log($"Received pong from server for ping {pingCount} and message {message}");
    }
}
