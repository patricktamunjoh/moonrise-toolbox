using System;
using UnityEngine;

namespace MoonriseGames.Toolbox.Types
{
    [Serializable]
    public class InspectorButton
    {
        [SerializeField]
        private bool _isSet;

        public bool IsClicked => GetAndConsumePress();
        public bool IsToggled => _isSet;

        private bool GetAndConsumePress()
        {
            var isPressed = _isSet;
            _isSet = false;
            return isPressed;
        }
    }
}
