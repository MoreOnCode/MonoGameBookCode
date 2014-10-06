/*
    <CustomDefinitions>
      <Platform Name="Android">TRACE;ANDROID;GLES;OPENGL;OPENAL</Platform>
      <Platform Name="Angle">TRACE;ANGLE;GLES;OPENGL;OPENAL</Platform>
      <Platform Name="iOS">IOS;GLES;OPENGL;OPENAL</Platform>
      <Platform Name="Linux">TRACE;LINUX;OPENGL;OPENAL</Platform>
      <Platform Name="MacOS">MONOMAC;OPENGL;OPENAL</Platform>
      <Platform Name="Ouya">TRACE;ANDROID;GLES;OPENGL;OUYA;OPENAL</Platform>
      <Platform Name="PSMobile">PSM</Platform>
      <Platform Name="Windows">TRACE;WINDOWS;DIRECTX;WINDOWS_MEDIA_SESSION</Platform>
      <Platform Name="Windows8">TRACE;NETFX_CORE;WINRT;WINDOWS_STOREAPP;DIRECTX;DIRECTX11_1;WINDOWS_MEDIA_ENGINE</Platform>
      <Platform Name="WindowsGL">TRACE;WINDOWS;OPENGL;OPENAL</Platform>
      <Platform Name="WindowsPhone">TRACE;SILVERLIGHT;WINDOWS_PHONE;WINRT;DIRECTX</Platform>
      <Platform Name="Web">TRACE;WEB</Platform>
    </CustomDefinitions>
*/
using System;

namespace Microsoft.Xna.Framework
{
	public enum Platforms {
		Android,
		Angle,
		iOS,
		Linux,
		MacOS,
		Ouya,
		PSMobile,
		Windows,
		Windows8,
		WindowsGL,
		WindowsPhone,
		Web,
		Unknown
	};

	public static class PlatformHelper {
		public static Platforms CurrentPlatform { 
			get {
				#if OUYA
					return Platforms.Ouya;
				#elif ANDROID
					return Platforms.Android;
				#elif ANGLE
					return Platforms.Angle;
				#elif IOS
					return Platforms.iOS;
				#elif LINUX
					return Platforms.Linux;
				#elif MONOMAC
					return Platforms.MacOS;
				#elif PSM
					return Platforms.PSMobile;
				#elif WINDOWS && OPENGL
					return Platforms.WindowsGL;
				#elif WINDOWS && DIRECTX
					return Platforms.Windows;
				#elif WINDOWS_STOREAPP && WINRT
					return Platforms.Windows8;
				#elif WINDOWS_PHONE
					return Platforms.WindowsPhone;
				#elif WEB
					return Platforms.Web;
				#else
					return Platforms.Unknown;
				#endif
			}
		}
			
		public static bool IsWindows { 
			get { 
				return 
					CurrentPlatform == Platforms.Windows || 
					CurrentPlatform == Platforms.WindowsGL || 
					CurrentPlatform == Platforms.Windows8; 
			} 
		}
		
		public static bool IsMacOS { 
			get{ return CurrentPlatform == Platforms.MacOS; } 
		}
		
		public static bool IsLinux { 
			get{ return CurrentPlatform == Platforms.Linux; } 
		}
		
		// TODO: assume keyboard and mouse support?
		public static bool IsDesktop { 
			get { return IsLinux || IsMacOS || IsWindows; } 
		}
		
		// TODO: assume touchscreen and accelerometer?
		public static bool IsMobile { 
			get { 
				return 
					CurrentPlatform == Platforms.WindowsPhone || 
					CurrentPlatform == Platforms.iOS || 
					CurrentPlatform == Platforms.Android ||
					CurrentPlatform == Platforms.PSMobile; 
			} 
		}
		
		// TODO: assume GamePad?
		public static bool IsConsole { 
			get { 
				return 
					CurrentPlatform == Platforms.Ouya ||
					CurrentPlatform == Platforms.PSMobile;
			} 
		}
		
		public static bool IsDirectX { 
			get { 
				#if DIRECTX
					return true; 
				#else
					return false;
				#endif
			} 
		}

		public static bool IsOpenGL { 
			get { 
				#if OPENGL || GLES
					return true; 
				#else
					return false;
				#endif
			} 
		}
		
		public static bool IsOpenGLES { 
			get { 
				#if GLES
					return true; 
				#else
					return false;
				#endif
			} 
		}
		
		public static bool IsOpenAL { 
			get { 
				#if OPENAL
					return true; 
				#else
					return false;
				#endif
			} 
		}
		
		public static bool IsDebug {
			get { 
				#if DEBUG
					return true; 
				#else
					return false;
				#endif
			} 
		}
		
		public static bool IsTrace {
			get { 
				#if TRACE
					return true; 
				#else
					return false;
				#endif
			} 
		}
	}
}