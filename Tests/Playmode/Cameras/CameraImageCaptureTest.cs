using System.Collections;
using System.IO;
using MoonriseGames.Toolbox.Cameras;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MoonriseGames.Toolbox.Playmode.Tests.Cameras
{
    public class CameraImageCaptureTest
    {
        private Camera Camera { get; set; }

        [SetUp]
        public void Setup()
        {
            Function.ClearScene();
            Camera = new GameObject().AddComponent<Camera>();
        }

        [UnityTest]
        public IEnumerator ShouldCaptureScreenshot()
        {
            var sut = Camera.gameObject.AddComponent<CameraImageCapture>();
            var file = Path.ChangeExtension(Path.GetTempFileName(), ".jpg");

            yield return null;
            sut.CaptureScreenshot(file);

            Assert.True(File.Exists(file));
        }

        [UnityTest]
        public IEnumerator ShouldCaptureScreenshotAsJpg()
        {
            var sut = Camera.gameObject.AddComponent<CameraImageCapture>();
            var file = Path.GetTempFileName();

            yield return null;
            sut.CaptureScreenshot(file);

            Assert.True(File.Exists(Path.ChangeExtension(file, ".jpg")));
        }

        [UnityTest]
        public IEnumerator ShouldIgnoreInvalidSizes()
        {
            var sut = Camera.gameObject.AddComponent<CameraImageCapture>();
            var file = Path.ChangeExtension(Path.GetTempFileName(), ".jpg");

            sut.ScreenshotWidth = 0;
            sut.ScreenshotHeight = 0;

            yield return null;
            sut.CaptureScreenshot(file);

            Assert.True(File.Exists(file));
        }

        [UnityTest]
        public IEnumerator ShouldProvideValidPath()
        {
            var sut = Camera.gameObject.AddComponent<CameraImageCapture>();
            var path = sut.GetScreenshotFilePathOnDesktop();

            yield return null;

            Assert.False(File.Exists(path));
            Assert.True(Path.HasExtension(path));
        }
    }
}
