using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using OpenCvSharp.Native;
using Java.Nio;

namespace OpenCvSharp.Android
{
    public class NativeBinding : Native.NativeBindings
    {
        public static NativeBinding K => (NativeBinding)Kernal;

        public static void Init(Context context, Activity activity, ImageView imShowTarget = null)
        {
            Kernal = new NativeBinding(context, activity, imShowTarget);
        }

        public Context AppContext;
        public Activity MainActivity;
        public ImageView ImShowTarget;

        public NativeBinding(Context context, Activity activity, ImageView imShowTarget = null)
        {
            AppContext = context;
            MainActivity = activity;
            ImShowTarget = imShowTarget;
        }

        public override void ImShow(string name, Mat m)
        {
            if (ImShowTarget != null)
            {
                Bitmap bit = Bitmap.CreateBitmap(m.Width, m.Height, Bitmap.Config.Argb8888);
                using (Mat mat = new Mat())
                {
                    Cv2.CvtColor(m, mat, ColorConversionCodes.BGR2RGB);

                    byte[] buffer = new byte[mat.Channel * mat.Total()];
                    mat.GetArray(0, 0, buffer);
                    using (var raw = ByteBuffer.Wrap(buffer))
                    {
                        bit.CopyPixelsFromBuffer(raw);
                    }
                    //Utils.MatToBitmap((Mat)mat.Object, bit);

                    MainActivity.RunOnUiThread(() =>
                    {
                        ImShowTarget.SetImageBitmap(bit);
                    });
                }
            }
        }

        public override int WaitKey(int sleep = 0)
        {
            System.Threading.Thread.Sleep(sleep);

            return 255;
        }

        public override void Sleep(int sleep = 0)
        {
            System.Threading.Thread.Sleep(sleep);
        }

        public override Capture NewCapture(int index)
        {
            return new AndroidCapture(index);
        }

        public override Capture NewCapture(string file)
        {
            return new AndroidCapture(file);
        }
    }
}