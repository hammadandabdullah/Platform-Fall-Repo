using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearbyPlayerChecker : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private float radius = 5f;

    private Collider[] colliders;
    private bool playerInRange = false;

    private void Update()
    {
        playerInRange = false;

        colliders = Physics.OverlapSphere(transform.position, radius, platformLayer);

        foreach (Collider col in colliders)
        {
            if(col != GetComponent<Collider>())
            {
                playerInRange = true;
            }
        }
    }

    public bool IsPlayerInRange()
    {
        return playerInRange;
    }

    public Transform GetPlayer()
    {
        if (colliders == null)
            return null;
        else if (colliders.Length == 0)
            return null;
        else
        {
            return colliders[Random.Range(0, colliders.Length)].transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
