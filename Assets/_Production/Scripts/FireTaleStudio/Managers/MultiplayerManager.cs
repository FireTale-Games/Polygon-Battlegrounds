using Unity.Netcode;

namespace FTS.Managers
{
    public class MultiplayerManager : BaseManager
    {
        public void SetNetworkConnection(bool value) => NetworkManager.Singleton.enabled = value;

        private void Start() => 
            SetNetworkConnection(false);
    }
}