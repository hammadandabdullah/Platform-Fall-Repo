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
            GameManager.Instance.DecrementPlayersCount();
            int currentPlayersCount = GameManager.Instance.GetRemainingPlayers();

            if (isPlayer)
            {
                UIManager.Instance.ShowLosePanel();
                GameManager.canMove = false;

                if (currentPlayersCount <= GameManager.minimumPlayersInFirstMode)
                {
                    ShowWinToPlayersRpc();
                }
            }
            else
            {
                if (currentPlayersCount <= GameManager.minimumPlayersInFirstMode)
                {
                    ShowWinToPlayersRpc();
                }

                if (IsServer)
                {
                    GetComponent<NetworkObject>().Despawn();
                }
            }
        }
    }

    [Rpc(SendTo.Everyone)]
    public void ShowWinToPlayersRpc()
    {
        UIManager.Instance.ShowWinPanel();
        GameManager.canMove = false;
    }
}
