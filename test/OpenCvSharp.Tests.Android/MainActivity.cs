using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace OpenCvSharp.Tests.Android
{
    [Activity(Label = "OpenCvSharp.Tests.Android", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        bool inited = false;
        int count = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            ImageView imgView = FindViewById<ImageView>(Resource.Id.imageView1);
            try
            {
                OpenCvSharp.Android.NativeBinding.Init(this, this, imgView);
            }
            catch(Exception ex)
            {
                Logger.Error(ex.ToString());
            }

            var cap = Native.NativeBindings.Kernal.NewCapture(0);
            cap.FrameReady += (sender, arg) =>
            {
                var m = arg.Mat;
                Cv2.ImShow("a", m);
            };
            cap.Start();

            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += delegate 
            {
                if (!inited)
                {
                    Cv2.Initialize();
                    inited = true;
                }

                Mat m = new Mat(3, 1, MatType.CV_64FC1, new double[] { 1, 2, 1 });
                Mat m2 = new Mat(1, 3, MatType.CV_64FC1, new double[] { 1, 2, 1 });
                var result = (m * m2).ToMat();
                var get = new double[result.Total()];
                result.GetArray(0, 0, get);
                System.Diagnostics.Debug.WriteLine($"Row:{result.Rows}, Col:{result.Cols}");

                button.Text = $"{count++} clicks! / {NativeMethods.core_Mat_sizeof()}\n";

                if (cap.IsRunning)
                {
                    cap.Stop();
                }
                else
                {
                    cap.Start();
                }
                button.Text += cap.IsRunning ? "Running" : "Nope";
            };
        }
    }
}

