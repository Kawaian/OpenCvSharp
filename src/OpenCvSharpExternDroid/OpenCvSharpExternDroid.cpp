#include "OpenCvSharpExternDroid.h"
#include "include_opencv.h"

#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO, "OpenCvSharpExternDroid", __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN, "OpenCvSharpExternDroid", __VA_ARGS__))

extern "C" {
	/* 이 Trivial 함수는 이 동적 네이티브 라이브러리가 컴파일되는 플랫폼 ABI를 반환합니다.*/
	const char * OpenCvSharpExternDroid::getPlatformABI()
	{
	#if defined(__arm__)
	#if defined(__ARM_ARCH_7A__)
	#if defined(__ARM_NEON__)
		#define ABI "armeabi-v7a/NEON"
	#else
		#define ABI "armeabi-v7a"
	#endif
	#else
		#define ABI "armeabi"
	#endif
	#elif defined(__i386__)
		#define ABI "x86"
	#else
		#define ABI "unknown"
	#endif
		LOGI("This dynamic shared library is compiled with ABI: %s", ABI);
		return "This native library is compiled with ABI: %s" ABI ".";
	}

	void OpenCvSharpExternDroid()
	{
	}

	OpenCvSharpExternDroid::OpenCvSharpExternDroid()
	{
	}

	OpenCvSharpExternDroid::~OpenCvSharpExternDroid()
	{
	}
}
