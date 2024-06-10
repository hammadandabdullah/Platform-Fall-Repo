using UnityEngine;
using Unity.Netcode;

public class PlayerInitialPosition : NetworkBehaviour
{
    public static bool canMove = false;

    void Start()
    {
        canMove = false;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            transform.position = SpawnPoints.Instance.spawnPoints[Random.Range(0, SpawnPoints.Instance.spawnPoints.Length)].position;
            GameManager.Instance.SetActiveCountDownTimer(true);
        }
    }
}