using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DownLimitCheck : NetworkBehaviour
{
    [SerializeField] private bool isPlayer;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DownLimit") && isPlayer)
        {
            Lose();
            WinOthersRpc();
            gameObject.SetActive(false);
        }
    }

    public void Lose()
    {
        UIManager.Instance.ShowLosePanel();
        PlayerInitialPosition.canMove = false;
    }

    [Rpc(SendTo.NotMe)]
    public void WinOthersRpc()
    {
        UIManager.Instance.ShowWinPanel();
        PlayerInitialPosition.canMove = false;
    }
}
