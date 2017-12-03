using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp.Native;
using System.Threading;

namespace OpenCvSharp.Windows
{
    public class WindowsCapture : Capture
    {
        public override bool IsOpened => capture != null;
        public override event EventHandler<FrameArgs> FrameReady;

        VideoCapture capture;
        Thread thread;

        public WindowsCapture(int index)
        {
            capture = new VideoCapture(index);
        }

        public WindowsCapture(string file)
        {
            capture = new VideoCapture(file);
        }

        public override void Dispose()
        {
            if(capture != null)
            {
                Stop();

                capture.Dispose();
                capture = null;
            }
        }

        protected override void OnStart()
        {
            OnStop();

            thread = new Thread(new ThreadStart(Proc));
            thread.Start();
        }

        private void Proc()
        {
            double lastMs = 0;
            double fps = capture.Get(CaptureProperty.Fps);
            char lastKey = (char)255;
            if (fps < 1)
            {
                fps = 30;
            }

            while (true)
            {
                Mat mat = new Mat();
                FrameArgs arg = new FrameArgs(mat, lastKey);
                if (capture.Read(mat) && !mat.Empty())
                {
                    FrameReady?.Invoke(this, arg);

                    int sleep = (int)Math.Max(1, (1000.0 / fps) - (Logger.Stopwatch.ElapsedMilliseconds - lastMs));
                    lastMs = Logger.Stopwatch.ElapsedMilliseconds;
                    lastKey = (char)Cv2.WaitKey(sleep);
                }
                else
                {
                    Thread.Sleep(1);
                }

                if (arg.MatDispose)
                {
                    mat.Dispose();
                }

                if (arg.Break)
                {
                    IsRunning = false;
                    return;
                }
            }
        }

        protected override void OnStop()
        {
            if(thread != null)
            {
                thread.Abort();
                thread = null;
            }
        }
    }

    public class NativeBindings : DefaultNativeBindings
    {
        public static void Init()
        {
            Kernal = new NativeBindings();
        }

        public override Capture NewCapture(int index)
        {
            return new WindowsCapture(index);
        }

        public override Capture NewCapture(string file)
        {
            return new WindowsCapture(file);
        }

        public override void Sleep(int sleep = 0)
        {
            Thread.Sleep(sleep);
        }
    }
}
