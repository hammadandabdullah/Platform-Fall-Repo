using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    public static CameraController Instance;

    [SerializeField] private Transform player;
    [SerializeField] private float rotationSpeed = 10f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (!IsOwner)
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        // Get the horizontal input axis (e.g., mapped to A/D keys or left/right arrows)
        float horizontalInput = Input.GetAxis("Mouse X");

        // Rotate the CameraRig around the y-axis
        transform.RotateAround(player.position, Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
        //Position
        transform.position = player.position;
    }
}
