using System.Collections;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private GameObject player;

    private float startTime;
    private float currentServerTime;
    private bool gameStarted = false;
    private int minimumPlayerCount = 2;
    private float countDownTotalTime = 3f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
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
            StartCoundDownRpc(startTime);
        }
    }

    [Rpc(SendTo.Everyone)]
    private void StartCoundDownRpc(float givenStartTime)
    {
        float serverTimeWhenGameStarted = currentServerTime - givenStartTime;
        float countDown = countDownTotalTime - serverTimeWhenGameStarted;
        countDownText.text = "count Down: " + countDown.ToString("0");

        if(countDown <= 0)
        {
            countDownText.gameObject.SetActive(false);
            PlayerMovement.canMove = true;
            VibratingPlatform.canVibrate = true;
        }
    }

    public void SetActiveCountDownTimer(bool active)
    {
        countDownText.gameObject.SetActive(active);
    }
}
