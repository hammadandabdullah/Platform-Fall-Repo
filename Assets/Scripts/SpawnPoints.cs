using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public static SpawnPoints Instance;
    public Transform[] spawnPoints;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}
