using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;

    public void OnClickHost()
    {
        networkManager.StartHost();    
    }

    public void OnClickClient()
    {
        networkManager.StartClient();
    }
}
