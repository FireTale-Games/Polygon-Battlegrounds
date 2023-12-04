using System.Linq;
using FTS.Tools.Attributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FTS.Managers
{
    [DisallowMultipleComponent]
    public class MenuPlayManager : BaseManager
    {
        [Dropdown(nameof(GetSceneNames))]
        [SerializeField] private string _selectedScene;
        
        private string[] GetSceneNames() =>
            EditorBuildSettings.scenes.Where(scene => scene.path != SceneManager.GetActiveScene().path)
                                      .Select(scene => System.IO.Path.GetFileNameWithoutExtension(scene.path))
                                      .ToArray();

        public void StartGame() => SceneManager.LoadScene(_selectedScene);
    }
}