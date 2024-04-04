using System.Collections;
using System.Threading;
using MoonriseGames.Toolbox.Working;
using UnityEngine;

namespace MoonriseGames.Toolbox.Cameras
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField]
        private float _shakeDuration = 0.2f;

        public float ShakeDuration
        {
            get => _shakeDuration;
            set => _shakeDuration = value;
        }

        [SerializeField]
        private float _shakePositionOffset = 0.1f;

        public float ShakePositionOffset
        {
            get => _shakePositionOffset;
            set => _shakePositionOffset = value;
        }

        [SerializeField, Range(4, 12)]
        private int _shakeIterations = 8;

        public int ShakeIterations
        {
            get => Mathf.Max(_shakeIterations, 4);
            set => _shakeIterations = value;
        }

        private IEnumerator Coroutine { get; set; }

        public void Shake(float intensity)
        {
            intensity = Mathf.Clamp01(intensity);

            if (Coroutine != null)
                StopCoroutine(Coroutine);

            if (intensity > 0)
                StartCoroutine(Coroutine = AnimateShake(intensity));

            if (intensity == 0)
                transform.localPosition = Vector3.zero;
        }

        private IEnumerator AnimateShake(float intensity)
        {
            var offset = ShakePositionOffset * intensity;
            var iterations = (int)Mathf.Lerp(4, ShakeIterations, intensity);

            var duration = ShakeDuration * intensity;
            var stepDuration = duration / iterations;

            var previousAngle = 0f;

            for (var i = 0; i <= iterations; i++)
            {
                previousAngle = Random.Range(previousAngle + 90, previousAngle + 270);

                var from = transform.localPosition;
                var to = Quaternion.Euler(0, 0, previousAngle) * Vector3.up * Mathf.Lerp(offset, 0, (float)i / iterations);

                yield return AnimateStep(from, to, stepDuration);
            }
        }

        private IEnumerator AnimateStep(Vector2 from, Vector2 to, float duration)
        {
            var timer = 0f;

            while (timer < duration)
            {
                timer = Mathf.Min(duration, timer + Time.deltaTime);
                transform.localPosition = Vector3.Lerp(from, to, timer / duration);
                yield return null;
            }
        }
    }
}
