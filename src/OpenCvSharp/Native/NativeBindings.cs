//Redirect some cv functions to native api or library.
//such as VideoCapture.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Native;

namespace OpenCvSharp
{
    public partial class Cv2
    {
        public static void ImShow(string name, Mat m)
        {
            NativeBindings.Kernal.ImShow(name, m);
        }

        public static int WaitKey(int sleep = 0)
        {
            return NativeBindings.Kernal.WaitKey(sleep);
        }
        
        public static void NamedWindow(string winname, WindowMode flags)
        {
            NativeBindings.Kernal.NamedWindow(winname, flags);
        }

        public static void DestroyAllWindows()
        {
            NativeBindings.Kernal.DestroyAllWindows();
        }
    }
}

namespace OpenCvSharp.Native
{
    public abstract class Capture : IDisposable
    {
        bool isRunning = false;
        public bool IsRunning 
        {
            get => isRunning;
            protected set
            {
                if(value != isRunning)
                {
                    isRunning = value;
                    if (value)
                        CaptureStarted?.Invoke(this, EventArgs.Empty);
                    else
                        CaptureStopped?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public virtual double FPS { get; set; }

        public abstract bool IsOpened { get; }
        public abstract event EventHandler<FrameArgs> FrameReady;
        public event EventHandler CaptureStarted;
        public event EventHandler CaptureStopped;
        
        public abstract void Dispose();
        public void Start()
        {
            IsRunning = true;
            OnStart();
        }
        protected abstract void OnStart();
        public void Stop()
        {
            IsRunning = false;
            OnStop();
        }
        protected abstract void OnStop();

        public virtual int Index { get; private set; } = -1;
        public virtual string FilePath { get; private set; } = null;


        public void Wait()
        {
            while (IsRunning)
            {
                NativeBindings.Kernal.Sleep(1);
            }
        }
        
        public static Capture New(int index)
        {
            var cap = NativeBindings.Kernal.NewCapture(index);
            cap.Index = index;
            return cap;
        }

        public static Capture New(string filePath)
        {
            var cap = NativeBindings.Kernal.NewCapture(filePath);
            cap.FilePath = filePath;
            return cap;
        }
    }

    public class FrameArgs : EventArgs
    {
        public Mat Mat { get; set; }
        public char LastKey { get; set; }
        /// <summary>
        /// By default, Mat will dispose after frame end.
        /// </summary>
        public bool MatDispose { get; set; } = true;
        /// <summary>
        /// Stop capture loop in frame event
        /// </summary>
        public bool Break { get; set; } = false;

        public FrameArgs(Mat mat, char k = (char)255)
        {
            Mat = mat;
            LastKey = k;
        }
    }

    public abstract class NativeBindings
    {
        private static NativeBindings kernal;
        public static NativeBindings Kernal
        {
            get => kernal;
            set { kernal = value; NativeMethods.TryPInvoke(); }
        }

        public abstract Capture NewCapture(int index);
        public abstract Capture NewCapture(string file);

        public abstract void NamedWindow(string winname, WindowMode flags);
        public abstract void DestroyAllWindows();
        public abstract void ImShow(string name, Mat m);
        public abstract int WaitKey(int sleep = 0);
        public abstract void Sleep(int sleep = 0);
        public void SleepEx(int sleep = 0)
        {
            double startMs = CvLogger.Stopwatch.Elapsed.TotalMilliseconds;
            while (true)
            {
                Sleep(1);
                if(CvLogger.Stopwatch.Elapsed.TotalMilliseconds - startMs > sleep - 0.7)
                {
                    return;
                }
            }
        }
    }

    public abstract class DefaultNativeBindings : NativeBindings
    {
        public override void DestroyAllWindows()
        {
            Cv2.PInvokeDestroyAllWindows();
        }

        public override void NamedWindow(string winname, WindowMode flags)
        {
            Cv2.PInvokeNamedWindow(winname, flags);
        }

        public override void ImShow(string name, Mat m)
        {
            Cv2.PInvokeImShow(name, m);
        }

        public override int WaitKey(int sleep = 0)
        {
            return Cv2.PInvokeWaitKey(sleep);
        }
    }
}
