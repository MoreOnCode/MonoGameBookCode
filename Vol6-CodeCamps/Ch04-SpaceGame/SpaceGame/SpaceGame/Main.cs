using System;
#if MONOMAC
using MonoMac.AppKit;
using MonoMac.Foundation;
#elif IPHONE
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

namespace SpaceGame
{
#if MONOMAC
	class Program
	{
		static void Main (string[] args)
		{
			NSApplication.Init ();

			using (var p = new NSAutoreleasePool ()) {
				NSApplication.SharedApplication.Delegate = new AppDelegate ();
				NSApplication.Main (args);
			}
		}
	}

	public partial class AppDelegate : NSApplicationDelegate
	{
		private SpaceGame game;

		public override void FinishedLaunching (MonoMac.Foundation.NSObject notification)
		{
			game = new SpaceGame();
			game.Run();
		}

		public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
		{
			return true;
		}
	}
#elif IPHONE
	[Register ("AppDelegate")]
	class Program : UIApplicationDelegate 
	{
		private PlatformerGame game;

		public override void FinishedLaunching (UIApplication app)
		{
			game = new PlatformerGame();
			game.Run();
		}

		static void Main (string [] args)
		{
			UIApplication.Main (args,null,"AppDelegate");
		}
	}
#else
	#if !WINDOWS_PHONE
	/// <summary>
	/// The main class.
	/// </summary>
	public static class Program
	{
		static void Main()
		{
		#if WINDOWS || LINUX || PSM
			using (var game = new PlatformerGame())
			game.Run();
		#else
			var factory = new MonoGame.Framework.GameFrameworkViewSource<PlatformerGame>();
			Windows.ApplicationModel.Core.CoreApplication.Run(factory);
		#endif
		}
	}
	#endif
#endif
}