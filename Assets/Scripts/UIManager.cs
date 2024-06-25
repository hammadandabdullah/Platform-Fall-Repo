using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private NetworkManager networkManager;

    [SerializeField] private TextMeshProUGUI joinCodeText;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void ShowWinPanel()
    {
        if (!losePanel.activeSelf)
            winPanel.SetActive(true);
    }

    public void ShowLosePanel()
    {
        if(!winPanel.activeSelf)
            losePanel.SetActive(true);
    }

    public void RestartGame()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene(0);
        Destroy(networkManager.gameObject);
    }

    public async void OnClickHost()
    {
        //networkManager.StartHost();    
        await StartHostWithRelay();
    }

    public async void OnClickClient()
    {
        //networkManager.StartClient();
        await StartClientWithRelay(joinCodeText.text.Substring(0, 6));
    }

    public async Task<string> StartHostWithRelay(int maxConnections = 5)
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        Debug.Log("JoinCode: " + joinCode);
        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    public async Task<bool> StartClientWithRelay(string joinCode)
    {
        Debug.Log("StartClientWithRelay - JoinCode - " + joinCode);

        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode: joinCode);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
        return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
    }
}
