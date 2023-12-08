using FTS.Data;
using FTS.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class SinglePlayerGameSelectionScreen : MenuScreenBase
    {
        [SerializeField] private Button _playGame;
        private string _mapName = "WorldOne_Scene";

        protected override void OnInitialize(IManager manager)
        {
            if (manager is MenuPlayManager menuPlayManager)
                BindToMenuPlayManager(menuPlayManager);
        }

        private void BindToMenuPlayManager(MenuPlayManager menuPlayManager)
        {
            foreach (IMapButtonUi mapButton in GetComponentsInChildren<IMapButtonUi>())
                mapButton.MapButton.onClick.AddListener(() => _mapName = mapButton.MapName);
            _playGame.onClick.RemoveAllListeners();
            _playGame.onClick.AddListener(() => StartGame(menuPlayManager));
        }

        private void StartGame(MenuPlayManager menuPlayManager)
        {
            menuPlayManager.SetMapSettings(new MapSettings(_mapName));
            SceneManager.LoadScene(menuPlayManager.GameSettings.MapSettings.r_mapName);
        }
    }
}