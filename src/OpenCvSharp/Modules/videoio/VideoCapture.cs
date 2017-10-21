using System;
using System.Runtime.InteropServices;

namespace OpenCvSharp
{
    public class VideoCapture : DisposableCvObject
    {
        private CaptureType captureType;

        #region Init and Disposal
        
        /// <summary>
        /// Initializes empty capture.
        /// To use this, you should call Open. 
        /// </summary>
        /// <returns></returns>
        public VideoCapture()
        {
            try
            {
                ptr = NativeMethods.videoio_VideoCapture_new1();
            }
            catch (Exception e)
            {
                throw new OpenCvSharpException("Failed to create VideoCapture", e);
            }
            if (ptr == IntPtr.Zero)
                throw new OpenCvSharpException("Failed to create VideoCapture");
            
            captureType = CaptureType.NotSpecified;
        }

        /// <summary>
        /// Allocates and initialized the CvCapture structure for reading a video stream from the camera. 
        /// Currently two camera interfaces can be used on Windows: Video for Windows (VFW) and Matrox Imaging Library (MIL); and two on Linux: V4L and FireWire (IEEE1394).
        /// </summary>
        /// <param name="index">Index of the camera to be used. If there is only one camera or it does not matter what camera to use -1 may be passed. </param>
        /// <returns></returns>
        public VideoCapture(int index)
        {
            try
            {
                ptr = NativeMethods.videoio_VideoCapture_new3(index);
            }
            catch (Exception e)
            {
                throw new OpenCvSharpException("Failed to create VideoCapture", e);
            }
            if (ptr == IntPtr.Zero)
            {
                throw new OpenCvSharpException("Failed to create VideoCapture");
            }
            captureType = CaptureType.Camera;
        }

        /// <summary>
        /// Allocates and initialized the CvCapture structure for reading a video stream from the camera. 
        /// Currently two camera interfaces can be used on Windows: Video for Windows (VFW) and Matrox Imaging Library (MIL); and two on Linux: V4L and FireWire (IEEE1394). 
        /// </summary>
        /// <param name="device">Device type</param>
        /// <returns></returns>
        public VideoCapture(CaptureDevice device)
            : this((int)device)
        {
        }

        /// <summary>
        /// Allocates and initialized the CvCapture structure for reading a video stream from the camera. 
        /// Currently two camera interfaces can be used on Windows: Video for Windows (VFW) and Matrox Imaging Library (MIL); and two on Linux: V4L and FireWire (IEEE1394). 
        /// </summary>
        /// <param name="device">Device type</param>
        /// <param name="index">Index of the camera to be used. If there is only one camera or it does not matter what camera to use -1 may be passed. </param>
        /// <returns></returns>
        public VideoCapture(CaptureDevice device, int index)
            : this((int)device + index)
        {
        }

        /// <summary>
        /// Allocates and initialized the CvCapture structure for reading a video stream from the camera. 
        /// Currently two camera interfaces can be used on Windows: Video for Windows (VFW) and Matrox Imaging Library (MIL); and two on Linux: V4L and FireWire (IEEE1394).
        /// </summary>
        /// <param name="index">Index of the camera to be used. If there is only one camera or it does not matter what camera to use -1 may be passed. </param>
        /// <returns></returns>
        public static VideoCapture FromCamera(int index)
        {
            return new VideoCapture(index);
        }

        /// <summary>
        /// Allocates and initialized the CvCapture structure for reading a video stream from the camera. 
        /// Currently two camera interfaces can be used on Windows: Video for Windows (VFW) and Matrox Imaging Library (MIL); and two on Linux: V4L and FireWire (IEEE1394). 
        /// </summary>
        /// <param name="device">Device type</param>
        /// <returns></returns>
        public static VideoCapture FromCamera(CaptureDevice device)
        {
            return new VideoCapture(device);
        }

        /// <summary>
        /// Allocates and initialized the CvCapture structure for reading a video stream from the camera. 
        /// Currently two camera interfaces can be used on Windows: Video for Windows (VFW) and Matrox Imaging Library (MIL); and two on Linux: V4L and FireWire (IEEE1394). 
        /// </summary>
        /// <param name="device">Device type</param>
        /// <param name="index">Index of the camera to be used. If there is only one camera or it does not matter what camera to use -1 may be passed. </param>
        /// <returns></returns>
        public static VideoCapture FromCamera(CaptureDevice device, int index)
        {
            return new VideoCapture((int)device + index);
        }

        /// <summary>
        /// Allocates and initialized the CvCapture structure for reading the video stream from the specified file.
        /// After the allocated structure is not used any more it should be released by cvReleaseCapture function. 
        /// </summary>
        /// <param name="fileName">Name of the video file. </param>
        /// <returns></returns>
        public VideoCapture(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
            /*if (!File.Exists(fileName))
                throw new FileNotFoundException("File not found", fileName);*/

            ptr = NativeMethods.videoio_VideoCapture_new2(fileName);

            if (ptr == IntPtr.Zero)
                throw new OpenCvSharpException("Failed to create VideoCapture");
            
            captureType = CaptureType.File;
        }

        /// <summary>
        /// Allocates and initialized the CvCapture structure for reading the video stream from the specified file.
        /// After the allocated structure is not used any more it should be released by cvReleaseCapture function. 
        /// </summary>
        /// <param name="fileName">Name of the video file. </param>
        /// <returns></returns>
        public static VideoCapture FromFile(string fileName)
        {
            return new VideoCapture(fileName);
        }

        /// <summary>
        /// Initializes from native pointer
        /// </summary>
        /// <param name="ptr">CvCapture*</param>
        protected internal VideoCapture(IntPtr ptr)
        {
            this.ptr = ptr;
        }

        /// <summary>
        /// Releases unmanaged resources
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            NativeMethods.videoio_VideoCapture_delete(ptr);
            base.DisposeUnmanaged();
        }

        #endregion

        /*
        #region Properties
        
        #region Basic
        /// <summary>
        /// Gets the capture type (File or Camera) 
        /// </summary>
        public CaptureType CaptureType
        {
            get { return captureType; }
        }

        /// <summary>
        /// Gets or sets film current position in milliseconds or video capture timestamp 
        /// </summary>
        public int PosMsec
        {
            get => (int)NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.PosMsec);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.PosMsec, value);
        }

        /// <summary>
        /// Gets or sets 0-based index of the frame to be decoded/captured next
        /// </summary>
        public int PosFrames
        {
            get => (int)NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.PosFrames);
            set
            {
                if (captureType == CaptureType.Camera)
                    throw new NotSupportedException("Only for video files");
                NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.PosFrames, value);
            }
        }
        
        /// <summary>
        /// Gets or sets relative position of video file
        /// </summary>
        public CapturePosAviRatio PosAviRatio
        {
            get => (CapturePosAviRatio)(int)NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.PosAviRatio);
            set
            {
                if (captureType == CaptureType.Camera)
                    throw new NotSupportedException("Only for video files");
                NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.PosAviRatio, (int)value);
            }
        }
        
        /// <summary>
        /// Gets or sets width of frames in the video stream
        /// </summary>
        public int FrameWidth
        {
            get => (int)NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.FrameWidth);
            set
            {
                if (captureType == CaptureType.File)
                    throw new NotSupportedException("Only for cameras");
                NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.FrameWidth, value);
            }
        }
        
        /// <summary>
        /// Gets or sets height of frames in the video stream 
        /// </summary>
        public int FrameHeight
        {
            get => (int)NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.FrameHeight);
            set
            {
                if (captureType == CaptureType.File)
                    throw new NotSupportedException("Only for cameras");
                NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.FrameHeight, value);
            }
        }
        
        /// <summary>
        /// Gets or sets frame rate
        /// </summary>
        public double Fps
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Fps);
            set
            {
                if (captureType == CaptureType.File)
                    throw new NotSupportedException("Only for cameras");
                NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Fps, value);
            }
        }

        /// <summary>
        /// Gets or sets 4-character code of codec 
        /// </summary>
        public string FourCC
        {
            get
            {
                int src = (int)NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.FourCC);
                IntBytes bytes = new IntBytes { Value = src };
                char[] fourcc = {
                    Convert.ToChar(bytes.B1),
                    Convert.ToChar(bytes.B2),
                    Convert.ToChar(bytes.B3),
                    Convert.ToChar(bytes.B4)
                };
                return new string(fourcc);
            }
            set
            {
                if (captureType == CaptureType.File)
                    throw new NotSupportedException("Only for cameras");
                if (value.Length != 4)
                    throw new ArgumentException("Length of the argument string must be 4");
                
                byte c1 = Convert.ToByte(value[0]);
                byte c2 = Convert.ToByte(value[1]);
                byte c3 = Convert.ToByte(value[2]);
                byte c4 = Convert.ToByte(value[3]);
                int v = FourCCCalcurator.Run(c1, c2, c3, c4);
                NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.FourCC, v);
            }
        }

        /// <summary>
        /// Gets number of frames in video file 
        /// </summary>
        public int FrameCount
        {
            get
            {
                return (int)NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.FrameCount);
            }
        }

        /// <summary>
        /// Gets or sets brightness of image (only for cameras) 
        /// </summary>
        public double Brightness
        {
            get
            {
                if (captureType == CaptureType.File)
                    throw new NotSupportedException("Only for cameras");
                return (int)NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Brightness);
            }
            set
            {
                if (captureType == CaptureType.File)
                    throw new NotSupportedException("Only for cameras");
                NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Brightness, value);
            }
        }

        /// <summary>
        /// Gets or sets contrast of image (only for cameras) 
        /// </summary>
        public double Contrast
        {
            get
            {
                if (captureType == CaptureType.File)
                    throw new NotSupportedException("Only for cameras");
                return (int)NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Contrast);
            }
            set
            {
                if (captureType == CaptureType.File)
                    throw new NotSupportedException("Only for cameras");
                NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Contrast, value);
            }
        }

        /// <summary>
        /// Gets or sets saturation of image (only for cameras) 
        /// </summary>
        public double Saturation
        {
            get
            {
                if (captureType == CaptureType.File)
                    throw new NotSupportedException("Only for cameras");
                return (int)NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Saturation);
            }
            set
            {
                if (captureType == CaptureType.File)
                    throw new NotSupportedException("Only for cameras");
                NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Saturation, value);
            }
        }

        /// <summary>
        /// Gets or sets hue of image (only for cameras) 
        /// </summary>
        public double Hue
        {
            get
            {
                if (captureType == CaptureType.File)
                    throw new NotSupportedException("Only for cameras");
                return (int)NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Hue);
            }
            set
            {
                if (captureType == CaptureType.File)
                    throw new NotSupportedException("Only for cameras");
                NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Hue, value);
            }
        }

        /// <summary>
        /// The format of the Mat objects returned by retrieve()
        /// </summary>
        public int Format
        {
            get => (int)NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Format);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Format, value);
        }

        /// <summary>
        /// A backend-specific value indicating the current capture mode
        /// </summary>
        public int Mode
        {
            get => (int)NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Mode);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Mode, value);
        }

        /// <summary>
        /// Gain of the image (only for cameras)
        /// </summary>
        public double Gain
        {
            get
            {
                if (captureType == CaptureType.File)
                    throw new NotSupportedException("Only for cameras");
                return NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Gain);
            }
            set
            {
                if (captureType == CaptureType.File)
                    throw new NotSupportedException("Only for cameras");
                NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Gain, value);
            }
        }

        /// <summary>
        /// Exposure (only for cameras)
        /// </summary>
        public double Exposure
        {
            get
            {
                if (captureType == CaptureType.File)
                    throw new NotSupportedException("Only for cameras");
                return NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Exposure);
            }
            set
            {
                if (captureType == CaptureType.File)
                    throw new NotSupportedException("Only for cameras");
                NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Exposure, value);
            }
        }

        /// <summary>
        /// Boolean flags indicating whether images should be converted to RGB
        /// </summary>
        public bool ConvertRgb
        {
            get => (int)NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.ConvertRgb) != 0;
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.ConvertRgb, value ? 0 : 1);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double WhiteBalanceBlueU
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.WhiteBalanceBlueU);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.WhiteBalanceBlueU, value);
        }

        /// <summary>
        /// TOWRITE (note: only supported by DC1394 v 2.x backend currently)
        /// </summary>
        public double Rectification
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Rectification);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Rectification, value);
        }

        public double Monocrome
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Monocrome);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Monocrome, value);
        }
        
        public double Sharpness
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Sharpness);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Sharpness, value);
        }

        /// <summary>
        /// exposure control done by camera,
        /// user can adjust refernce level using this feature
        /// [CV_CAP_PROP_AUTO_EXPOSURE]
        /// </summary>
        public double AutoExposure
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.AutoExposure);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.AutoExposure, value);
        }

        public double Gamma
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Gamma);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Gamma, value);
        }

        /// <summary>
        /// [CV_CAP_PROP_TEMPERATURE]
        /// </summary>
        public double Temperature
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Temperature);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Temperature, value);
        }
        
        public double Trigger
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Trigger);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Trigger, value);
        }

        public double TriggerDelay
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.TriggerDelay);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.TriggerDelay, value);
        }

        public double WhiteBalanceRedV
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.WhiteBalanceRedV);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.WhiteBalanceRedV, value);
        }

        public double Zoom
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Zoom);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Zoom, value);
        }

        public double Focus
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Focus);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Focus, value);
        }

        public double Guid
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Guid);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Guid, value);
        }

        public double IsoSpeed
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.IsoSpeed);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.IsoSpeed, value);
        }

        public double BackLight
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.BackLight);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.BackLight, value);
        }

        public double Pan
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Pan);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Pan, value);
        }

        public double Tilt
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Tilt);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Tilt, value);
        }

        public double Roll
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Roll);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Roll, value);
        }

        public double Iris
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Iris);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Iris, value);
        }

        public double Settings
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.Settings);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.Settings, value);
        }

        public double BufferSize
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.BufferSize);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.BufferSize, value);
        }
        
        public bool AutoFocus
        {
            get => (int)NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.AutoFocus) != 0;
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.AutoFocus, value ? 1 : 0);
        }
        #endregion

        #region OpenNI
        /// <summary>
        /// [CV_CAP_PROP_OPENNI_OUTPUT_MODE]
        /// </summary>
        public double OpenNI_OutputMode
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.OpenNI_OutputMode);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.OpenNI_OutputMode, value);
        }

        /// <summary>
        /// in mm
        /// [CV_CAP_PROP_OPENNI_FRAME_MAX_DEPTH]
        /// </summary>
        public double OpenNI_FrameMaxDepth
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.OpenNI_FrameMaxDepth);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.OpenNI_FrameMaxDepth, value);
        }

        /// <summary>
        /// in mm
        /// [CV_CAP_PROP_OPENNI_BASELINE]
        /// </summary>
        public double OpenNI_Baseline
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.OpenNI_Baseline);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.OpenNI_Baseline, value);
        }

        /// <summary>
        /// in pixels
        /// [CV_CAP_PROP_OPENNI_FOCAL_LENGTH]
        /// </summary>
        public double OpenNI_FocalLength
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.OpenNI_FocalLength);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.OpenNI_FocalLength, value);
        }

        /// <summary>
        /// flag that synchronizes the remapping depth map to image map
        /// by changing depth generator's view point (if the flag is "on") or
        /// sets this view point to its normal one (if the flag is "off").
        /// [CV_CAP_PROP_OPENNI_REGISTRATION]
        /// </summary>
        public double OpenNI_Registration
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.OpenNI_Registration);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.OpenNI_Registration, value);
        }

        /// <summary>
        /// [CV_CAP_OPENNI_IMAGE_GENERATOR_OUTPUT_MODE]
        /// </summary>
        public double OpenNI_ImageGeneratorOutputMode
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.OpenNI_ImageGeneratorOutputMode);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.OpenNI_ImageGeneratorOutputMode, value);
        }

        /// <summary>
        /// [CV_CAP_OPENNI_DEPTH_GENERATOR_BASELINE]
        /// </summary>
        public double OpenNI_DepthGeneratorBaseline
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.OpenNI_DepthGeneratorBaseline);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.OpenNI_DepthGeneratorBaseline, value);
        }

        /// <summary>
        /// [CV_CAP_OPENNI_DEPTH_GENERATOR_FOCAL_LENGTH]
        /// </summary>
        public double OpenNI_DepthGeneratorFocalLength
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.OpenNI_DepthGeneratorFocalLength);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.OpenNI_DepthGeneratorFocalLength, value);
        }

        /// <summary>
        /// [CV_CAP_OPENNI_DEPTH_GENERATOR_REGISTRATION_ON]
        /// </summary>
        public double OpenNI_DepthGeneratorRegistrationON
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.OpenNI_DepthGeneratorRegistrationON);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.OpenNI_DepthGeneratorRegistrationON, value);
        }

        #endregion
        
        #region GStreamer

        /// <summary>
        /// default is 1
        /// [CV_CAP_GSTREAMER_QUEUE_LENGTH]
        /// </summary>
        public double GStreamerQueueLength
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.GStreamerQueueLength);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.GStreamerQueueLength, value);
        }

        /// <summary>
        /// ip for anable multicast master mode. 0 for disable multicast
        /// [CV_CAP_PROP_PVAPI_MULTICASTIP]
        /// </summary>
        public double PvAPIMulticastIP
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.PvAPIMulticastIP);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.PvAPIMulticastIP, value);
        }
        #endregion
        
        #region XI
        /// <summary>
        /// Change image resolution by binning or skipping.  
        /// [CV_CAP_PROP_XI_DOWNSAMPLING]
        /// </summary>
        public double XI_Downsampling
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_Downsampling);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_Downsampling, value);
        }

        /// <summary>
        /// Output data format.
        /// [CV_CAP_PROP_XI_DATA_FORMAT]
        /// </summary>
        public double XI_DataFormat
        {
            get
            {
                return NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_DataFormat);
            }
        }

        /// <summary>
        /// Horizontal offset from the origin to the area of interest (in pixels).
        /// [CV_CAP_PROP_XI_OFFSET_X]
        /// </summary>
        public double XI_OffsetX
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_OffsetX);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_OffsetX, value);
        }

        /// <summary>
        /// Vertical offset from the origin to the area of interest (in pixels).
        /// [CV_CAP_PROP_XI_OFFSET_Y]
        /// </summary>
        public double XI_OffsetY
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_OffsetY);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_OffsetY, value);
        }

        /// <summary>
        /// Defines source of trigger.
        /// [CV_CAP_PROP_XI_TRG_SOURCE]
        /// </summary>
        public double XI_TrgSource
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_TrgSource);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_TrgSource, value);
        }

        /// <summary>
        /// Generates an internal trigger. PRM_TRG_SOURCE must be set to TRG_SOFTWARE.
        /// [CV_CAP_PROP_XI_TRG_SOFTWARE]
        /// </summary>
        public double XI_TrgSoftware
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_TrgSoftware);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_TrgSoftware, value);
        }

        /// <summary>
        /// Selects general purpose input
        /// [CV_CAP_PROP_XI_GPI_SELECTOR]
        /// </summary>
        public double XI_GpiSelector
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_GpiSelector);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_GpiSelector, value);
        }

        /// <summary>
        /// Set general purpose input mode
        /// [CV_CAP_PROP_XI_GPI_MODE]
        /// </summary>
        public double XI_GpiMode
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_GpiMode);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_GpiMode, value);
        }

        /// <summary>
        /// Get general purpose level
        /// [CV_CAP_PROP_XI_GPI_LEVEL]
        /// </summary>
        public double XI_GpiLevel
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_GpiLevel);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_GpiLevel, value);
        }

        /// <summary>
        /// Selects general purpose output 
        /// [CV_CAP_PROP_XI_GPO_SELECTOR]
        /// </summary>
        public double XI_GpoSelector
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_GpoSelector);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_GpoSelector, value);
        }

        /// <summary>
        /// Set general purpose output mode
        /// [CV_CAP_PROP_XI_GPO_MODE]
        /// </summary>
        public double XI_GpoMode
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_GpoMode);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_GpoMode, value);
        }

        /// <summary>
        /// Selects camera signalling LED 
        /// [CV_CAP_PROP_XI_LED_SELECTOR]
        /// </summary>
        public double XI_LedSelector
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_LedSelector);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_LedSelector, value);
        }

        /// <summary>
        /// Define camera signalling LED functionality
        /// [CV_CAP_PROP_XI_LED_MODE]
        /// </summary>
        public double XI_LedMode
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_LedMode);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_LedMode, value);
        }

        /// <summary>
        /// Calculates White Balance(must be called during acquisition)
        /// [CV_CAP_PROP_XI_MANUAL_WB]
        /// </summary>
        public double XI_ManualWB
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_ManualWB);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_ManualWB, value);
        }

        /// <summary>
        /// Automatic white balance
        /// [CV_CAP_PROP_XI_AUTO_WB]
        /// </summary>
        public double XI_AutoWB
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_AutoWB);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_AutoWB, value);
        }

        /// <summary>
        /// Automatic exposure/gain
        /// [CV_CAP_PROP_XI_AEAG]
        /// </summary>
        public double XI_AEAG
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_AEAG);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_AEAG, value);
        }

        /// <summary>
        /// Exposure priority (0.5 - exposure 50%, gain 50%).
        /// [CV_CAP_PROP_XI_EXP_PRIORITY]
        /// </summary>
        public double XI_ExpPriority
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_ExpPriority);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_ExpPriority, value);
        }

        /// <summary>
        /// Maximum limit of exposure in AEAG procedure
        /// [CV_CAP_PROP_XI_AE_MAX_LIMIT]
        /// </summary>
        public double XI_AEMaxLimit
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_AEMaxLimit);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_AEMaxLimit, value);
        }

        /// <summary>
        /// Maximum limit of gain in AEAG procedure
        /// [CV_CAP_PROP_XI_AG_MAX_LIMIT]
        /// </summary>
        public double XI_AGMaxLimit
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_AGMaxLimit);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_AGMaxLimit, value);
        }

        /// <summary>
        /// default is 1
        /// [CV_CAP_PROP_XI_AEAG_LEVEL]
        /// </summary>
        public double XI_AEAGLevel
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_AEAGLevel);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_AEAGLevel, value);
        }

        /// <summary>
        /// default is 1
        /// [CV_CAP_PROP_XI_TIMEOUT]
        /// </summary>
        public double XI_Timeout
        {
            get => NativeMethods.videoio_VideoCapture_get(ptr, (int)CaptureProperty.XI_Timeout);
            set => NativeMethods.videoio_VideoCapture_set(ptr, (int)CaptureProperty.XI_Timeout, value);
        }
        #endregion

        #endregion
        */

        #region Methods
        /// <summary>
        /// Retrieves the specified property of camera or video file. 
        /// </summary>
        /// <param name="propertyId">property identifier.</param>
        /// <returns>property value</returns>
        public double Get(CaptureProperty propertyId)
        {
            return NativeMethods.videoio_VideoCapture_get(ptr, (int)propertyId);
        }

        /// <summary>
        /// Retrieves the specified property of camera or video file. 
        /// </summary>
        /// <param name="propertyId">property identifier.</param>
        /// <returns>property value</returns>
        public double Get(int propertyId)
        {
            return NativeMethods.videoio_VideoCapture_get(ptr, propertyId);
        }

        /// <summary>
        /// Grabs the frame from camera or file. The grabbed frame is stored internally. 
        /// The purpose of this function is to grab frame fast that is important for syncronization in case of reading from several cameras simultaneously. 
        /// The grabbed frames are not exposed because they may be stored in compressed format (as defined by camera/driver). 
        /// To retrieve the grabbed frame, cvRetrieveFrame should be used. 
        /// </summary>
        /// <returns></returns>
        public bool Grab()
        {
            ThrowIfDisposed();
            return NativeMethods.videoio_VideoCapture_grab(ptr) != 0;
        }

        /// <summary>
        /// Decodes and returns the grabbed video frame.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="channel">non-zero streamIdx is only valid for multi-head camera live streams</param>
        /// <returns></returns>
        public bool Retrieve(Mat image, int channel = 0)
        {
            ThrowIfDisposed();
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            image.ThrowIfDisposed();
            return NativeMethods.videoio_VideoCapture_retrieve(ptr, image.CvPtr, channel) != 0;
        }

        /// <summary>
        /// Returns the pointer to the image grabbed with cvGrabFrame function. 
        /// The returned image should not be released or modified by user. 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="streamIdx">non-zero streamIdx is only valid for multi-head camera live streams</param>
        /// <returns></returns>
        public bool Retrieve(Mat image, CameraChannels streamIdx = CameraChannels.OpenNI_DepthMap)
        {
            ThrowIfDisposed();
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            image.ThrowIfDisposed();
            return NativeMethods.videoio_VideoCapture_retrieve(ptr, image.CvPtr, (int)streamIdx) != 0;
        }

        /// <summary>
        /// Decodes and returns the grabbed video frame.
        /// </summary>
        /// <returns></returns>
        public Mat RetrieveMat()
        {
            ThrowIfDisposed();

            var mat = new Mat();
            NativeMethods.videoio_VideoCapture_operatorRightShift_Mat(ptr, mat.CvPtr);
            return mat;
        }

        /// <summary>
        /// Grabs a frame from camera or video file, decompresses and returns it. 
        /// This function is just a combination of cvGrabFrame and cvRetrieveFrame in one call. 
        /// The returned image should not be released or modified by user. 
        /// </summary>
        /// <returns></returns>
        public bool Read(Mat image)
        {
            ThrowIfDisposed();
            if(image == null)
                throw new ArgumentNullException(nameof(image));
            image.ThrowIfDisposed();
            
            //NativeMethods.videoio_VideoCapture_read(ptr, image.CvPtr);
            /*
            bool grabbed = NativeMethods.videoio_VideoCapture_grab(ptr) != 0;
            if (!grabbed)
                return false;
            */
            NativeMethods.videoio_VideoCapture_operatorRightShift_Mat(ptr, image.CvPtr);
            GC.KeepAlive(image);
            return true;
        }

        /// <summary>
        /// Sets the specified property of video capturing.
        /// </summary>
        /// <param name="propertyId">property identifier. </param>
        /// <param name="value">value of the property. </param>
        /// <returns></returns>
        public int Set(CaptureProperty propertyId, double value)
        {
            return NativeMethods.videoio_VideoCapture_set(ptr, (int)propertyId, value);
        }

        /// <summary>
        /// Sets the specified property of video capturing.
        /// </summary>
        /// <param name="propertyId">property identifier. </param>
        /// <param name="value">value of the property. </param>
        /// <returns></returns>
        public int Set(int propertyId, double value)
        {
            return NativeMethods.videoio_VideoCapture_set(ptr, propertyId, value);
        }

        /// <summary>
        /// Opens the specified video file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public void Open(string fileName)
        {
            ThrowIfDisposed();
            NativeMethods.videoio_VideoCapture_open1(ptr, fileName);
            captureType = CaptureType.File;
        }

        /// <summary>
        /// Allocates and initialized the CvCapture structure for reading a video stream from the camera. 
        /// Currently two camera interfaces can be used on Windows: Video for Windows (VFW) and Matrox Imaging Library (MIL); and two on Linux: V4L and FireWire (IEEE1394).
        /// </summary>
        /// <param name="index">Index of the camera to be used. If there is only one camera or it does not matter what camera to use -1 may be passed. </param>
        /// <returns></returns>
        public void Open(int index)
        {
            ThrowIfDisposed();
            try
            {
                NativeMethods.videoio_VideoCapture_open2(ptr, index);
            }
            catch (Exception e)
            {
                throw new OpenCvSharpException("Failed to create CvCapture", e);
            }
            captureType = CaptureType.Camera;
        }

        /// <summary>
        /// Allocates and initialized the CvCapture structure for reading a video stream from the camera. 
        /// Currently two camera interfaces can be used on Windows: Video for Windows (VFW) and Matrox Imaging Library (MIL); and two on Linux: V4L and FireWire (IEEE1394). 
        /// </summary>
        /// <param name="device">Device type</param>
        /// <returns></returns>
        public void Open(CaptureDevice device)
        {
            Open((int)device);
        }

        /// <summary>
        /// Allocates and initialized the CvCapture structure for reading a video stream from the camera. 
        /// Currently two camera interfaces can be used on Windows: Video for Windows (VFW) and Matrox Imaging Library (MIL); and two on Linux: V4L and FireWire (IEEE1394). 
        /// </summary>
        /// <param name="device">Device type</param>
        /// <param name="index">Index of the camera to be used. If there is only one camera or it does not matter what camera to use -1 may be passed. </param>
        /// <returns></returns>
        public void Open(CaptureDevice device, int index)
        {
            Open((int)device + index);
        }

        /// <summary>
        /// Closes video file or capturing device.
        /// </summary>
        /// <returns></returns>
        public void Release()
        {
            ThrowIfDisposed();
            NativeMethods.videoio_VideoCapture_release(ptr);
        }

        /// <summary>
        /// Returns true if video capturing has been initialized already.
        /// </summary>
        /// <returns></returns>
        public bool IsOpened()
        {
            ThrowIfDisposed();
            return NativeMethods.videoio_VideoCapture_isOpened(ptr) != 0;
        }
        #endregion
        
        [StructLayout(LayoutKind.Explicit)]
        private struct IntBytes
        {
            [FieldOffset(0)]
            public int Value;
            [FieldOffset(0)]
            public readonly byte B1;
            [FieldOffset(1)]
            public readonly byte B2;
            [FieldOffset(2)]
            public readonly byte B3;
            [FieldOffset(3)]
            public readonly byte B4;
        }
    }
}