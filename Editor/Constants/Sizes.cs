using UnityEditor;

namespace MoonriseGames.Toolbox.Editor.Constants
{
    public static class Sizes
    {
        public static float SpacingHorizontal => 5;
        public static float SpacingHorizontalSmall => 2;
        public static float SpacingVertical => EditorGUIUtility.standardVerticalSpacing;

        public static float WidthLabel => EditorGUIUtility.labelWidth;
        public static float WidthLabelSmall => 80;

        public static float WidthElement => EditorGUIUtility.fieldWidth;
    }
}
