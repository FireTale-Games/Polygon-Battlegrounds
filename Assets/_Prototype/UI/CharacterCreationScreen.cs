using UnityEngine;

namespace FTS.UI.Screens
{
    internal sealed class CharacterCreationScreen : MenuScreenBase
    {
        [SerializeField] private GameObject _camera;

        protected override void Awake()
        {
            base.Awake();
            _camera.SetActive(false);
        }

        public override void Show(float? speed = null)
        {
            base.Show(speed);
            _camera.SetActive(true);
        }

        public override void Hide(float? speed = null)
        {
            base.Hide(speed);
            _camera.SetActive(false);
        }
    }
}