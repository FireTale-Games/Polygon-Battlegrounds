using TMPro;
using UnityEngine;

internal sealed class LobbyCodeUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lobbyCodeLabel;
    
    public void SetDefaultValues(bool isHost, string lobbyCode)
    {
        if (isHost)
            _lobbyCodeLabel.text = lobbyCode;
        
        gameObject.SetActive(isHost);
    }
}
