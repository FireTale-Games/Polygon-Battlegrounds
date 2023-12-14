using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI
{
    internal sealed class MapSelectUi : MonoBehaviour
    {
        [SerializeField] private RectTransform _mapList;
        private float _spacing;
        
        private void Awake() => 
            _spacing = _mapList.GetComponent<VerticalLayoutGroup>().spacing;

        public void SetDefaultValues(bool isHost, RectTransform[] _maps)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
                Destroy(transform.GetChild(i));
            
            if (isHost)
            {
                foreach (RectTransform map in _maps)
                    Instantiate(map, transform);

                _mapList.sizeDelta = new Vector2(_mapList.sizeDelta.x, _maps[0].sizeDelta.y * _maps.Length + (_maps.Length - 1) * _spacing);
            }
            
            gameObject.SetActive(isHost);
        }
    }
}