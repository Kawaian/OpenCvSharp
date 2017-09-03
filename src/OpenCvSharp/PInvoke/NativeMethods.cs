using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Xamarin.Forms;
#if DOTNET_FRAMEWORK
using System.Security;
using System.Security.Permissions;
#endif
using OpenCvSharp.Util;

// ReSharper disable InconsistentNaming
#pragma warning disable 1591

namespace OpenCvSharp
{
    /// <summary>
    /// P/Invoke methods of OpenCV 2.x C++ interface
    /// </summary>
#if DOTNET_FRAMEWORK && !net20
    [SuppressUnmanagedCodeSecurity]
#endif
    public static partial class NativeMethods
    {
        /// <summary>
        /// Is tried P/Invoke once
        /// </summary>
        private static bool tried = false;

        //public const string DllVCRuntime = "vcruntime140";
        //public const string DllMsvcp = "msvcp140";
        public const string DllExtern = "OpenCvSharpExtern";
        public const string Version = "320";

        public const string DllFfmpegX86 = "opencv_ffmpeg" + Version;
        public const string DllFfmpegX64 = DllFfmpegX86 + "_64";

        /// <summary>
        /// Static constructor
        /// </summary>
#if DOTNET_FRAMEWORK
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
#endif
        static NativeMethods()
        {
            // call cv to enable redirecting 
            TryPInvoke();
        }

        /// <summary>
        /// Checks whether PInvoke functions can be called
        /// </summary>
        public static void TryPInvoke()
        {
            if (tried)
                return;
            tried = true;

            try
            {
                core_Mat_sizeof();
            }
            catch (DllNotFoundException e)
            {
                var exception = PInvokeHelper.CreateException(e);
                try
                {
                    Debug.WriteLine(exception.Message);
                }
                catch { }
                throw exception;
            }
            catch (BadImageFormatException e)
            {
                var exception = PInvokeHelper.CreateException(e);
                try
                {
                    Debug.WriteLine(exception.Message);
                }
                catch { }
                throw exception;
            }
            catch (Exception e)
            {
                Exception ex = e.InnerException ?? e;
                try
                {
                    Debug.WriteLine(ex.Message);
                }
                catch { }
                throw;
            }
        }

        /// <summary>
        /// Returns whether the OS is Windows or not
        /// </summary>
        /// <returns></returns>
        public static bool IsWindows()
        {
            return !IsUnix();
        }

        /// <summary>
        /// Returns whether the OS is *nix or not
        /// </summary>
        /// <returns></returns>
        public static bool IsUnix()
        {
            return Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android;
        }

        /// <summary>
        /// Returns whether the runtime is Mono or not
        /// </summary>
        /// <returns></returns>
        public static bool IsMono()
        {
            return (Type.GetType("Mono.Runtime") != null);
        }

#region Error redirection
        /// <summary>
        /// Custom error handler to be thrown by OpenCV
        /// </summary>
        public static readonly CvErrorCallback ErrorHandlerThrowException =
            delegate(ErrorCode status, string funcName, string errMsg, string fileName, int line, IntPtr userdata)
            {
                try
                {
                    //cvSetErrStatus(CvStatus.StsOk);
                    return 0;
                }
                finally
                {
                    throw new OpenCVException(status, funcName, errMsg, fileName, line);
                }
            };

        /// <summary>
        /// Custom error handler to ignore all OpenCV errors
        /// </summary>
        public static readonly CvErrorCallback ErrorHandlerIgnorance =
            delegate(ErrorCode status, string funcName, string errMsg, string fileName, int line, IntPtr userdata)
            {
                //cvSetErrStatus(CvStatus.StsOk);
                return 0;
            };

        /// <summary>
        /// Default error handler
        /// </summary>
        public static CvErrorCallback ErrorHandlerDefault;
#endregion
    }
}