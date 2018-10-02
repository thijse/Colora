using System;
using System.Windows.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Forms;

using Point = System.Windows.Point;
using Color = System.Windows.Media.Color;
using System.Drawing.Drawing2D;

namespace Colora
{
    class MouseScreenCapture
    {
        private DispatcherTimer timer;
        private int _currentCaptureSize;
        private Bitmap _map;
        private Bitmap _screen;
        private int _prevWidth;

        public event EventHandler CaptureTick;
        /// <summary>
        /// The real pixel size captured which will be zoomed to 100px.
        /// </summary>
        public int RealCaptureSize { get; set; }
        public Point MouseScreenPosition { get { return new Point(Control.MousePosition.X, Control.MousePosition.Y); } }
        public Bitmap CaptureBitmap { get; private set; }
        public BitmapImage CaptureBitmapImage
        {
            get
            {
                BitmapImage bmpImage = new BitmapImage();
                using (MemoryStream memory = new MemoryStream())
                {
                    CaptureBitmap.Save(memory, ImageFormat.Bmp);
                    memory.Position = 0;
                    bmpImage.BeginInit();
                    bmpImage.StreamSource = memory;
                    bmpImage.CacheOption = BitmapCacheOption.OnLoad;
                    bmpImage.EndInit();
                }
                return bmpImage;
            }
        }
        public Color PointerPixelColor { get; private set; }

        public MouseScreenCapture()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            timer.Tick += new EventHandler(timer_Tick);
            RealCaptureSize = 33;
            _map = new Bitmap(100, 100);
        }

        public void StartCapturing()
        {
            _currentCaptureSize = RealCaptureSize;
            timer.Start();
        }

        public void StartSinglePixelCapturing()
        {
            _currentCaptureSize = 1;
            timer.Start();
        }

        public void StopCapturing()
        {
            timer.Stop();
        }

        private Bitmap GetScreen(int width)
        {
            
            if (width != _prevWidth || _screen == null)
            {
                _screen = new Bitmap(width, width);
                _prevWidth = width;
            }
            return _screen;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            int sz = _currentCaptureSize;
            int shalf = (int)Math.Floor(_currentCaptureSize / (double)2);
            Bitmap screen = GetScreen(sz);
            using (Graphics g = Graphics.FromImage(screen))
            {
                g.CopyFromScreen(Control.MousePosition.X - shalf, Control.MousePosition.Y - shalf, 0, 0, new System.Drawing.Size(sz, sz));
            }

            using (Graphics g = Graphics.FromImage(_map))
            {
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(screen, new Rectangle(0, 0, 100, 100), new Rectangle(0, 0, sz, sz), GraphicsUnit.Pixel);
            }
            CaptureBitmap = _map;
            //CaptureBitmap = (Bitmap)_map.Clone();
            System.Drawing.Color drawing_Col = screen.GetPixel(sz / 2, sz / 2);
            PointerPixelColor = Color.FromRgb(drawing_Col.R, drawing_Col.G, drawing_Col.B);
            //screen.Dispose();
            if (CaptureTick != null)
                CaptureTick(this, new EventArgs());
        }
    }
}
