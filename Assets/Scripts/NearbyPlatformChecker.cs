using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearbyPlatformChecker : MonoBehaviour
{
    private VibratingPlatform platform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<VibratingPlatform>())
        {
            platform = other.GetComponent<VibratingPlatform>();
        }
    }

    public VibratingPlatform GetPlatform()
    {
        return platform;
    }
}
