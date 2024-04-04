using MoonriseGames.Toolbox.Constants;
using UnityEngine;

namespace MoonriseGames.Toolbox.Organization
{
    public class HierarchyMarker : MonoBehaviour
    {
        [SerializeField]
        private Color _color = Colors.MOONRISE_GAMES;
        public Color Color => _color;

        public bool IsSeparator => name.Contains("--");
    }
}
