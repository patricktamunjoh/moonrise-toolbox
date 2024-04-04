using System;
using System.IO;
using UnityEngine;

namespace MoonriseGames.Toolbox.Cameras
{
    //TODO: Add support for additional file types and settings
    [RequireComponent(typeof(Camera))]
    public class CameraImageCapture : MonoBehaviour
    {
        [SerializeField]
        private int _screenshotWidth;
        public int ScreenshotWidth
        {
            get => Mathf.Max(1, _screenshotWidth);
            set => _screenshotWidth = value;
        }

        [SerializeField]
        private int _screenshotHeight;
        public int ScreenshotHeight
        {
            get => Mathf.Max(1, _screenshotHeight);
            set => _screenshotHeight = value;
        }

        private Camera Camera { get; set; }

        private void Awake() => Camera = GetComponent<Camera>();

        public void CaptureScreenshot(string filePath)
        {
            var texture = CaptureCameraToTexture();
            WriteTextureToFile(filePath, texture);
        }

        private Texture2D CaptureCameraToTexture()
        {
            var renderTexture = new RenderTexture(ScreenshotWidth, ScreenshotHeight, 24);
            var texture = new Texture2D(ScreenshotWidth, ScreenshotHeight, TextureFormat.RGB24, false);

            Camera.targetTexture = renderTexture;
            Camera.Render();
            Camera.targetTexture = null;

            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, ScreenshotWidth, ScreenshotHeight), 0, 0);
            RenderTexture.active = null;

            Destroy(renderTexture);
            return texture;
        }

        private void WriteTextureToFile(string filePath, Texture2D texture)
        {
            filePath = Path.ChangeExtension(filePath, ".jpg");
            File.WriteAllBytes(filePath, texture.EncodeToJPG(100));
            Destroy(texture);
        }

        public string GetScreenshotFilePathOnDesktop() =>
            Path.Join(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), GetScreenshotFileName());

        public string GetScreenshotFileName() => $"Screenshot-{DateTime.Now.Ticks}.jpg";
    }
}
