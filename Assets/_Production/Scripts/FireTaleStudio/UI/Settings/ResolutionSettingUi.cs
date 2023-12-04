using System;
using System.Linq;
using UnityEngine;

namespace FTS.UI.Settings
{
    public readonly struct GameResolutions
    {
        public readonly int _width;
        public readonly int _height;

        public GameResolutions(int width, int height)
        {
            _width = width;
            _height = height;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is GameResolutions resolution)
                return _width == resolution._width && _height == resolution._height;

            return false;
        }

        public override int GetHashCode() => 
            _width.GetHashCode() ^ _height.GetHashCode();
    }
    
    internal sealed class ResolutionSettingUi : SettingPrevNextBaseUi
    {
        public override object Value => _value ?? Screen.resolutions.Select(r => new GameResolutions(r.width, r.height)).Distinct().ToArray().Length - 1;
        private GameResolutions[] _gameResolutions;

        protected override void InitializeButtons(Action<ISetting> onValueChange)
        { 
            int value = Convert.ToInt32(Value);
            
            _previousButton.onClick.AddListener(() => UpdateResolutionIndex(false));
            _nextButton.onClick.AddListener(() => UpdateResolutionIndex(true));
            return;
            
            void UpdateResolutionIndex(bool isIncrement)
            {
                value = isIncrement ? (value + 1) % _gameResolutions.Length :
                    value == 0 ? _gameResolutions.Length - 1 : value - 1;

                _value = value;
                onValueChange?.Invoke(this);
            }
        }

        protected override void InitializeValue(Action<ISetting> onValueChange, object prevNextValue)
        {
            _gameResolutions = Screen.resolutions.Select(r => new GameResolutions(r.width, r.height)).Distinct().ToArray();
            _value = prevNextValue ?? _gameResolutions.Length - 1;
            onValueChange?.Invoke(this);
        }

        public override void ApplyData()
        {
            byte value = Convert.ToByte(Value);
            Screen.SetResolution(_gameResolutions[value]._width, _gameResolutions[value]._height, Screen.fullScreenMode);
            _valueText.text = $"{_gameResolutions[value]._width} x {_gameResolutions[value]._height}";
        }
    }
}