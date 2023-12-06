using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class GameSelectionScreen : MenuScreenBase
    {
        [SerializeField] private Button _playGame;

        protected override void Awake()
        {
            base.Awake();
            _playGame.onClick.AddListener(() => SceneManager.LoadScene("EmptyTemplate_Scene"));
        }
    }
}