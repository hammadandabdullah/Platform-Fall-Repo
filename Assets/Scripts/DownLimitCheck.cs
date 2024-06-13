using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DownLimitCheck : NetworkBehaviour
{
    [SerializeField] private bool isPlayer;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DownLimit"))
        {
            if (isPlayer)
            {
                Lose();
                WinOthersRpc();
                gameObject.SetActive(false);
            }
            else if (IsServer) //If Bot and Is Server
            {
                GetComponent<NetworkObject>().Despawn();
            }
        }
    }

    public void Lose()
    {
        UIManager.Instance.ShowLosePanel();
        GameManager.canMove = false;
    }

    [Rpc(SendTo.NotMe)]
    public void WinOthersRpc()
    {
        UIManager.Instance.ShowWinPanel();
        GameManager.canMove = false;
    }
}
