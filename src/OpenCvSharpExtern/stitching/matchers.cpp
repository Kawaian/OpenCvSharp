// MattMatt2000: Custom file to expose some missing stuff
// from opencv_world320.
#pragma once
#include <opencv2/stitching/detail/matchers.hpp>
#include <opencv2/opencv.hpp>
#include <opencv2/core/base.hpp>

using namespace cv;
using namespace detail;

// Assuming HAVE_OPENCV_XFEATURES2D is defined.
// Should be the case anyway

SurfFeaturesFinderGpu::SurfFeaturesFinderGpu(double hess_thresh, int num_octaves, int num_layers,
    int num_octaves_descr, int num_layers_descr)
{
    surf_.keypointsRatio = 0.1f;
    surf_.hessianThreshold = hess_thresh;
    surf_.extended = false;
    num_octaves_ = num_octaves;
    num_layers_ = num_layers;
    num_octaves_descr_ = num_octaves_descr;
    num_layers_descr_ = num_layers_descr;
}

void SurfFeaturesFinderGpu::find(InputArray image, ImageFeatures &features)
{
    CV_Assert(image.depth() == CV_8U);

    ensureSizeIsEnough(image.size(), image.type(), image_);
    image_.upload(image);

    ensureSizeIsEnough(image.size(), CV_8UC1, gray_image_);
    
    // No CUDA support
    cv::cvtColor(image_, gray_image_, COLOR_BGR2GRAY);

    surf_.nOctaves = num_octaves_;
    surf_.nOctaveLayers = num_layers_;
    surf_.upright = false;
    surf_(gray_image_, cuda::GpuMat(), keypoints_);

    surf_.nOctaves = num_octaves_descr_;
    surf_.nOctaveLayers = num_layers_descr_;
    surf_.upright = true;
    surf_(gray_image_, cuda::GpuMat(), keypoints_, descriptors_, true);
    surf_.downloadKeypoints(keypoints_, features.keypoints);

    descriptors_.download(features.descriptors);
}

void SurfFeaturesFinderGpu::collectGarbage()
{
    surf_.releaseMemory();
    image_.release();
    gray_image_.release();
    keypoints_.release();
    descriptors_.release();
}