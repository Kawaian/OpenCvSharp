using System;
using System.Runtime.InteropServices;
using Xamarin.Forms;

#pragma warning disable 1591

namespace OpenCvSharp.Util
{
    // ReSharper disable once InconsistentNaming
    internal enum OS
    {
        Windows,
        Unix
    }
    internal enum Runtime
    {
        DotNet,
        Mono
    }

    /// <summary>
    /// Provides information for the platform which the user is using 
    /// </summary>
    internal static class Platform
    {
        /// <summary>
        /// OS type
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static readonly OS OS;
        /// <summary>
        /// Runtime type
        /// </summary>
        public static readonly Runtime Runtime;

        static Platform()
        {
            switch (Device.OS)
            {
                case TargetPlatform.Other:
                case TargetPlatform.iOS:
                case TargetPlatform.Android:
                    OS = OS.Unix;
                    break;
                case TargetPlatform.WinPhone:
                case TargetPlatform.Windows:
                    OS = OS.Windows;
                    break;
                default:
                    throw new Exception("Unknown OS");
            }
            Runtime = (Type.GetType("Mono.Runtime") == null) ? Runtime.Mono : Runtime.DotNet;
        }
    }
}
