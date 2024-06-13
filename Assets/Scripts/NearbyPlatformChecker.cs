using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearbyPlatformChecker : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private float radius = 5f;

    private Collider[] colliders;

    private void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, radius, platformLayer);
    }

    public Transform GetPlatform()
    {
        if (colliders == null)
            return null;
        else if (colliders.Length == 0)
            return null;
        else
        {
            //Try to find a non vibrating platform
            for (int i = 0; i < colliders.Length; i++)
            {
                VibratingPlatform currentPlatform = colliders[i].transform.GetComponent<VibratingPlatform>();

                if (!currentPlatform.IsVibrating())
                {
                    return currentPlatform.transform;
                }
            }

            //If all platforms are vibrating, then get a random one
            return colliders[Random.Range(0, colliders.Length)].transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
