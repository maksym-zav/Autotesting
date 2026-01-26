using Logs.TestInfo;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Logs.Loggers
{
    /// <summary>
    /// Logs screenshots
    /// </summary>
    internal class ScreenshotLogger : ILogger
    {
        public const int SM_CXSCREEN = 0; // Screen width
        public const int SM_CYSCREEN = 1; // Screen height
        private int height;
        private int width;

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int smIndex);

        public ScreenshotLogger()
        {
            width = GetSystemMetrics(SM_CXSCREEN);
            height = GetSystemMetrics(SM_CYSCREEN);
        }

        public void Log(object sender, TestRunData data)
        {
            if (data.LastStep.LastMessage?.MessageType == MessageCategory.Screenshot)
            {
                using (Bitmap bitmap = new Bitmap(width, height))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.CopyFromScreen(Point.Empty, Point.Empty, new Size(width, height));
                    }

                    bitmap.Save(data.LastStep.LastMessage.MessageText, ImageFormat.Png);
                }
            }
        }
    }
}
