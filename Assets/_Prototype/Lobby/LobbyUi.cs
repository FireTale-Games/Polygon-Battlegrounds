using FTS.Multiplayer;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUi : MonoBehaviour
{
    [SerializeField] private Button _createLobbyButton;
    [SerializeField] private Button joinLobbyButton;
    [SerializeField] private string _sceneName;

    private void Awake()
    {
        _createLobbyButton.onClick.AddListener(CreateGame);
        joinLobbyButton.onClick.AddListener(JoinGame);
    }

    private async void CreateGame()
    {
        await Multiplayer.Instance.CreateLobby();
        Loader.LoadNetwork(_sceneName);
    }
    
    private async void JoinGame()
    {
        await Multiplayer.Instance.QuickJoinLobby();
    }
}

public static class Loader
{
    public static void LoadNetwork(string sceneName) => 
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
}