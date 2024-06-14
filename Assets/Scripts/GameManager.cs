using System.Collections;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    public static bool canMove = false;
    public static int minimumPlayersInFirstMode = 5;

    [SerializeField] private TextMeshProUGUI topText;

    private float startTime;
    private float currentServerTime;
    private bool gameStarted = false;
    private int minimumPlayerCount = 2;
    private float countDownTotalTime = 3f;

    private int totalPlayers = 15;
    private NetworkVariable<int> playersDiedCount_NetworkVar = new NetworkVariable<int>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        canMove = false;
    }

    private void Update()
    {
        float halfRTTinSeconds =
            (NetworkManager.Singleton.LocalTime.TimeAsFloat - NetworkManager.Singleton.ServerTime.TimeAsFloat) / 2.0f;

        currentServerTime = halfRTTinSeconds + NetworkManager.Singleton.ServerTime.TimeAsFloat;

        //serverTimeWhenJoined = 5
        //countup = currentServerTime - serverTimeWhenJoined

        

        if (IsServer && !gameStarted)
        {
            if (NetworkManager.ConnectedClientsList.Count >= minimumPlayerCount)
            {
                startTime = currentServerTime;
                gameStarted = true;
            }
        }

        if (gameStarted)
        {
            StartCountDownRpc(startTime);
        }
    }

    [Rpc(SendTo.Everyone)]
    private void StartCountDownRpc(float givenStartTime)
    {
        float serverTimeWhenGameStarted = currentServerTime - givenStartTime;
        float countDown = countDownTotalTime - serverTimeWhenGameStarted;
        topText.text = "count Down: " + countDown.ToString("0");

        if(countDown <= 0)
        {
            //topText.gameObject.SetActive(false);
            UpdatePlayerRemainingText();
            canMove = true;
        }
    }

    public void SetActiveCountDownTimer(bool active)
    {
        topText.gameObject.SetActive(active);
    }

    public void DecrementPlayersCount()
    {
        if (IsServer)
        {
            playersDiedCount_NetworkVar.Value++;
        }

        UpdatePlayerRemainingText();
    }

    public int GetRemainingPlayers()
    {
        return totalPlayers - playersDiedCount_NetworkVar.Value;
    }

    private void UpdatePlayerRemainingText()
    {
        topText.text = "Players Remaining: " + GetRemainingPlayers().ToString();
    }
}
