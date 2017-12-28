﻿//Redirect some cv functions to native api or library.
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

        public void Wait()
        {
            while (IsRunning)
            {
                NativeBindings.Kernal.Sleep(1);
            }
        }
        
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
