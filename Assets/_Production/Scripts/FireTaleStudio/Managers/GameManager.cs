using System;
using System.Collections;
using UnityEngine;

namespace FTS.Managers
{
    [DisallowMultipleComponent]
    internal sealed class GameManager : MonoBehaviour
    {
        internal static GameManager Instance;

        [SerializeField] private BaseManager[] _managers;

        public Action<IManager> OnInitialize;

        private void Awake() => Initialize();

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            foreach (IManager manager in GetComponentsInChildren<IManager>())
                OnInitialize?.Invoke(manager);
        }

        private void Initialize()
        {
            Instance = this;
            foreach (BaseManager manager in _managers)
                Instantiate(manager, transform);
        }
    }
}