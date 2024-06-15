using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class VibratingPlatform : NetworkBehaviour
{
    [SerializeField] private float downSpeed = 10f;

    private float vibrationTime = 5f;
    private float downTime = 1f;
    private float respawnTime = 1f;
    private Animator anim;
    private bool collided = false;
    private bool isVibrating = false;
    private bool canGoDown = false;
    private bool isActive = true;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private Vector3 initialParentPosition;

    void Start()
    {
        anim = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        initialParentPosition = transform.parent.position;
    }

    private void Update()
    {
        if (collided && GameManager.canMove && !isVibrating)
        {
            StartVibratingRpc(true);
            collided = false;
        }

        if (canGoDown)
        {
            transform.parent.position -= new Vector3(0, downSpeed * Time.deltaTime, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collided = true;
            if (GameManager.canMove)
            {
                StartVibratingRpc(true);
            }
        }
    }

    [Rpc(SendTo.Everyone)]
    private void StartVibratingRpc(bool canVibrate)
    {
        anim.SetBool("vibration", canVibrate);

        if (!isVibrating)
        {
            Invoke(nameof(GoDown), vibrationTime);
            Invoke(nameof(TurnOffPlatform), vibrationTime + downTime);
            Invoke(nameof(TurnOnPlatform), vibrationTime + downTime + respawnTime);
            isVibrating = true;
        }
    }


    private void GoDown()
    {
        canGoDown = true;
    }

    private void TurnOffPlatform()
    {
        meshRenderer.enabled = false;
        meshCollider.enabled = false;
        isActive = false;
        canGoDown = false;
    }

    private void TurnOnPlatform()
    {
        meshRenderer.enabled = true;
        meshCollider.enabled = true;
        anim.SetBool("vibration", false);

        isActive = true;
        isVibrating = false;
        collided = false;

        transform.parent.position = initialParentPosition;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public bool IsVibrating()
    {
        return isVibrating;
    }
}
