using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public abstract class NativeBindings
    {
        public static NativeBindings Kernal { get; private set; }

        static NativeBindings()
        {
            Kernal = new DefaultNativeBindings();
        }

        public static void Init(NativeBindings native)
        {
            Kernal = native;
        }

        public abstract void ImShow(string name, Mat m);
        public abstract int WaitKey(int sleep = 0);
    }

    public class DefaultNativeBindings : NativeBindings
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
