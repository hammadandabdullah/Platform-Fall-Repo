using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private NetworkManager networkManager;

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

    public void OnClickHost()
    {
        networkManager.StartHost();    
    }

    public void OnClickClient()
    {
        networkManager.StartClient();
    }
}
