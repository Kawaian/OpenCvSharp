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
    }
}

namespace OpenCvSharp.Native
{
    public abstract class Capture : IDisposable
    {
        public bool IsRunning { get; protected set; }

        public virtual double FPS { get; set; }

        public abstract bool IsOpened { get; }
        public abstract event EventHandler<FrameArgs> FrameReady;

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
        
        public static Capture New(int index)
        {
            return NativeBindings.Kernal.NewCapture(index);
        }

        public static Capture New(string filePath)
        {
            return NativeBindings.Kernal.NewCapture(filePath);
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

        public FrameArgs(Mat mat, char k = (char)0)
        {
            Mat = mat;
            LastKey = k;
        }
    }

    public abstract class NativeBindings
    {
        public static NativeBindings Kernal { get; set; }

        public abstract Capture NewCapture(int index);
        public abstract Capture NewCapture(string file);

        public abstract void ImShow(string name, Mat m);
        public abstract int WaitKey(int sleep = 0);
        public abstract void Sleep(int sleep = 0);
    }

    public abstract class DefaultNativeBindings : NativeBindings
    {
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
