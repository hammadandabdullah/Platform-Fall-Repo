using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DownLimitCheck : NetworkBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DownLimit"))
        {
            Lose();
            WinOthersRpc();
            gameObject.SetActive(false);
        }
    }

    public void Lose()
    {
        UIManager.Instance.ShowLosePanel();
    }

    [Rpc(SendTo.NotMe)]
    public void WinOthersRpc()
    {
        UIManager.Instance.ShowWinPanel();
    }
}
