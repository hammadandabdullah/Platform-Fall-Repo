using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BotsSpawner : NetworkBehaviour
{
    [SerializeField] private int totalBots = 15;
    [SerializeField] private GameObject botPrefab;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            for (int i = 0; i < totalBots; i++)
            {
                GameObject newBot = Instantiate(botPrefab);
                newBot.transform.position = SpawnPoints.Instance.spawnPoints[Random.Range(0, SpawnPoints.Instance.spawnPoints.Length)].position;
                newBot.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}
