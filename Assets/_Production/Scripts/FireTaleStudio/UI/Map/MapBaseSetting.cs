using System;
using UnityEngine;

namespace FTS.UI.Map
{
    internal abstract class MapBaseSetting : MonoBehaviour, IMapSetting
    {
        public abstract void Initialize(Action onChangeData);
        public abstract void SetDefaultValues(bool isHost);
    }

    internal interface IMapSetting
    {
        public void Initialize(Action onChangeData);
        public void SetDefaultValues(bool isHost);
    }
}