using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp.Native;

namespace OpenCvSharp.Windows
{
    public class NativeBindings : Native.DefaultNativeBindings
    {
        public static void Init()
        {
            Kernal = new NativeBindings();
        }

        public override Capture NewCapture(int index)
        {
            throw new NotImplementedException();
        }

        public override Capture NewCapture(string file)
        {
            throw new NotImplementedException();
        }

        public override void Sleep(int sleep = 0)
        {
            System.Threading.Thread.Sleep(sleep);
        }
    }
}
