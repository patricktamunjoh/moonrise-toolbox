using System;
using UnityEngine;

namespace MoonriseGames.Toolbox.Extensions
{
    public static class NumberExtensions
    {
        public static bool IsSimilar(this float value, float other) => Mathf.Approximately(value, other);

        public static string ToRoman(this int number)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException();

            if (number >= 40)
                throw new NotImplementedException();

            var literal = "";
            var tens = number / 10;

            literal += new string('X', tens);
            number -= tens * 10;

            if (number is 9 or 4)
                literal += "I";

            if (number == 9)
                literal += "X";
            else if (number >= 4)
                literal += "V";

            if (number is 6 or 7 or 8)
                literal += new string('I', number - 5);

            if (number is 1 or 2 or 3)
                literal += new string('I', number);

            return literal;
        }

        public static float NormalizeAngle(this float angle)
        {
            angle = (angle % 360 + 360) % 360;
            return angle > 180 ? angle - 360 : angle;
        }
    }
}
