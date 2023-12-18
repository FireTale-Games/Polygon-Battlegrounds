using System;
using System.Collections.Generic;
using FTS.Data.Map;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.GameLobby
{
    internal sealed class MapSelectionUi : MonoBehaviour
    {
        [SerializeField] private SelectMapUi selectMapUi;
        [SerializeField] private RectTransform _mapList;
        private float _spacing;
        private Action<int> OnUpdateLobbyData;
        
        private void Awake() => 
            _spacing = _mapList.GetComponent<VerticalLayoutGroup>().spacing;

        public void Initialize(Action<int> onUpdateLobbyData) => 
            OnUpdateLobbyData = onUpdateLobbyData;

        public void SetDefaultValues(bool isHost, List<GameMap> _maps)
        {
            for (int i = _mapList.childCount - 1; i >= 0; i--)
                Destroy(_mapList.GetChild(i).gameObject);
            
            if (isHost)
            {
                foreach (GameMap gameMap in _maps)
                {
                    SelectMapUi selectMap = Instantiate(selectMapUi, _mapList.transform);
                    selectMap.Initialize(gameMap, OnUpdateLobbyData);
                }
                
                _mapList.sizeDelta = new Vector2(_mapList.sizeDelta.x, selectMapUi.GetComponent<RectTransform>().sizeDelta.y * _maps.Count + (_maps.Count - 1) * _spacing);
            }
            
            gameObject.SetActive(isHost);
        }
        
        private void OnDestroy() => OnUpdateLobbyData = null;
    }
}