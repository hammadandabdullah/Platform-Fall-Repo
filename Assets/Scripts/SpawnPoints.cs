using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public static SpawnPoints Instance;
    private SpawnPoint[] spawnPoints;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        spawnPoints = FindObjectsOfType<SpawnPoint>();
    }

    public Transform GetRandom()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
    }

    public Transform GetSpawnPoint(int i)
    {
        return spawnPoints[i].transform;
    }
}
