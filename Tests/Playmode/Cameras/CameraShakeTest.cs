using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoonriseGames.Toolbox.Cameras;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Working;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MoonriseGames.Toolbox.Playmode.Tests.Cameras
{
    public class CameraShakeTest
    {
        [UnityTest]
        public IEnumerator ShouldMoveDuringShake()
        {
            var sut = new GameObject().AddComponent<CameraShake>();

            sut.ShakeDuration = 1;
            sut.ShakePositionOffset = 1;
            sut.Shake(1);

            yield return new WaitForSeconds(0.1f);

            Assert.False(Vector3.zero.IsSimilar(sut.transform.localPosition));
        }

        [UnityTest]
        public IEnumerator ShouldReturnToZeroPosition()
        {
            var sut = new GameObject().AddComponent<CameraShake>();

            sut.ShakeDuration = 0.05f;
            sut.ShakePositionOffset = 1;
            sut.Shake(1);

            yield return new WaitForSeconds(0.1f);

            Assert.True(Vector3.zero.IsSimilar(sut.transform.localPosition));
        }

        [UnityTest]
        public IEnumerator ShouldRespectIntensity()
        {
            var sut = new GameObject().AddComponent<CameraShake>();

            sut.ShakeDuration = 0.05f;
            sut.ShakePositionOffset = 1;
            sut.Shake(0);

            yield return new WaitForSeconds(0.1f);

            Assert.True(Vector3.zero.IsSimilar(sut.transform.localPosition));
        }

        [UnityTest]
        public IEnumerator ShouldOverrideForSubsequentShake()
        {
            var sut = new GameObject().AddComponent<CameraShake>();

            sut.ShakeDuration = 0.05f;
            sut.ShakePositionOffset = 1;
            sut.Shake(1);
            sut.Shake(0);

            yield return new WaitForSeconds(0.1f);

            Assert.True(Vector3.zero.IsSimilar(sut.transform.localPosition));
        }

        [UnityTest]
        public IEnumerator ShouldNotExceedSetOffset()
        {
            var sut = new GameObject().AddComponent<CameraShake>();
            var points = new List<Vector3>();

            sut.ShakeDuration = 0.1f;
            sut.ShakePositionOffset = 1;
            sut.Shake(100);

            new WorkBuilder(sut, 0.1f).OnProgress(_ => points.Add(sut.transform.localPosition)).Build().Start();
            yield return new WaitForSeconds(0.1f);

            Assert.True(points.All(x => x.magnitude <= 1));
        }
    }
}
