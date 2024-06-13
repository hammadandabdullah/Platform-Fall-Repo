using UnityEngine;
using Unity.Netcode;

public class PlayerInitialPosition : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            transform.position = SpawnPoints.Instance.GetRandom().position;
            GameManager.Instance.SetActiveCountDownTimer(true);
        }
    }
}