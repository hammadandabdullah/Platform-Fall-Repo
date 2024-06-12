using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class VibratingPlatform : NetworkBehaviour
{
    public static bool canVibrate = false;
    private float vibrationTime = 5f;
    private float respawnTime = 2f;
    private Animator anim;
    private bool collided = false;
    private bool isVibrating = false;
    private bool isActive = true;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    void Start()
    {
        anim = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        canVibrate = true;
    }

    private void Update()
    {
        if (collided && canVibrate && !isVibrating)
        {
            StartVibratingRpc(true);
            collided = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collided = true;
            if (canVibrate)
            {
                Debug.Log("canVibrate");
                StartVibratingRpc(true);
            }
        }
    }

    [Rpc(SendTo.Everyone)]
    private void StartVibratingRpc(bool canVibrate)
    {
        anim.SetBool("vibration", canVibrate);

/*        if (!isVibrating)
        {
            Invoke(nameof(TurnOffPlatform), vibrationTime);
            Invoke(nameof(TurnOnPlatform), vibrationTime + respawnTime);
            isVibrating = true;
        }*/
    }

    private void TurnOffPlatform()
    {
        meshRenderer.enabled = false;
        meshCollider.enabled = false;
        isActive = false;
    }

    private void TurnOnPlatform()
    {
        meshRenderer.enabled = true;
        meshCollider.enabled = true;
        anim.SetBool("vibration", false);

        isActive = true;
        isVibrating = false;
        collided = false;
    }

    public bool IsActive()
    {
        return isActive;
    }
}
