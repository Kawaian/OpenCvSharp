using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCvSharp.Tests.Windows
{
    class Command
    {
        public string Help { get; set; }
        Action action;

        public Command(string help, Action action)
        {
            Help = help;

            this.action = action;
        }

        public void Run()
        {
            action.Invoke();
        }
    }

    class Program
    {
        static Stopwatch sw;
        static OpenFileDialog ofd;

        static Program()
        {
            //IMPORTANT
            OpenCvSharp.Windows.NativeBindings.Init();

            sw = new Stopwatch();
            sw.Start();
            ofd = new OpenFileDialog();
        }

        static Dictionary<string, Command> cmdDict = new Dictionary<string, Command>()
        {
            {
                "cls", new Command("Clear Console", ()=>
                {
                    Console.Clear();
                })
            },
            {
                "img", new Command("Open Image Test", ()=>
                {
                    if(ofd.ShowDialog() == DialogResult.OK)
                    {
                        using(Mat img = Cv2.ImRead(ofd.FileName))
                        {
                            Cv2.ImShow("img", img);
                            Cv2.WaitKey(0);
                        }
                    }
                })
            },
            {
                "cap", new Command("Video Capture Test", ()=>
                {
                    Console.Write("Index (if empty, open video) >>> ");
                    var read = Console.ReadLine();

                    Native.Capture cap = null;

                    if (string.IsNullOrWhiteSpace(read))
                    {
                        if(ofd.ShowDialog() == DialogResult.OK)
                        {
                            cap = Native.NativeBindings.Kernal.NewCapture(ofd.FileName);
                        }
                    }
                    else
                    {
                        int ind = int.MaxValue;
                        try
                        {
                            ind = Convert.ToInt32(read);
                        }
                        catch { }
                        if(ind != int.MaxValue)
                        {
                            cap = Native.NativeBindings.Kernal.NewCapture(ind);
                        }
                    }

                    cap.FrameReady += (o, a)=>
                    {
                        Cv2.ImShow("capture", a.Mat);
                        if(a.LastKey == 'e')
                        {
                            a.Break = true;
                        }
                    };
                    cap.Start();
                    cap.Wait();
                    cap.Dispose();
                })
            },
            {
                "info", new Command("Cv build informations", new Action(()=>
                {
                    Console.WriteLine(Cv2.GetBuildInformation());
                }))
            }
        };

        [STAThread]
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write(">>> ");
                var read = Console.ReadLine();
                var readLower = read.ToLower();
                switch (readLower)
                {
                    case "exit":
                        Environment.Exit(0);
                        break;
                    case "help":
                        Console.WriteLine("OpenCvSharp Windows Test Console App\n");
                        Console.WriteLine($" {"CMD".PadRight(12)}   Help Message");
                        Console.WriteLine("==============================================================");
                        foreach (var item in cmdDict)
                        {
                            Console.WriteLine($" {item.Key.ToUpper().PadRight(12)} | {item.Value.Help}");
                        }
                        break;
                    default:
                        if (cmdDict.ContainsKey(readLower))
                        {
                            cmdDict[readLower].Run();
                            Cv2.DestroyAllWindows();
                        }
                        else
                        {
                            Console.WriteLine($"Unknown command: {readLower}");
                        }
                        break;
                }
            }
        }
    }
}
