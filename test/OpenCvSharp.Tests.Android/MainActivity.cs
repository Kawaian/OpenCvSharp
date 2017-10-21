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

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += delegate 
            {
                try
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

                    button.Text = $"{count++} clicks! / {NativeMethods.core_Mat_sizeof()}";

                    VideoCapture cp = new VideoCapture(0);
                    Mat frame = new Mat();
                    var ret = cp.Read(frame);
                    System.Diagnostics.Debug.WriteLine("ret: " + ret);
                    System.Diagnostics.Debug.WriteLine("rett: " + frame.Rows);
                    System.Diagnostics.Debug.WriteLine("he: " + frame.At<byte>(123));
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("=========== ERROR OCCURED WHILE LOAD LIB ============\n"+ex.ToString());
                    Toast.MakeText(this, ex.ToString(), ToastLength.Long);
                }
            };
        }
    }
}

