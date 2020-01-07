using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Foundation;
using MedicationAlert.Services;

[assembly: Xamarin.Forms.Dependency(typeof(MedicationAlert.iOS.Logger))]
namespace MedicationAlert.iOS
{
	public class Logger : ILogger
	{

		// Simple Hack to use NSLog as NSLog is not provided anymore by Foundation
		[DllImport("/System/Library/Frameworks/Foundation.framework/Foundation")]
		extern static void NSLog(IntPtr format, [MarshalAs(UnmanagedType.LPStr)] string s);

		public static void NSLog(string format, params object[] args)
		{
			var fmt = NSString.CreateNative("%s");
			var val = (args == null || args.Length == 0) ? format : string.Format(format, args);

			NSLog(fmt, val);
			NSString.ReleaseNative(fmt);
		}

		public void D(object obj, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
		{
			NSLog("|---------------------------------------------------------------------");
			NSLog($"|{member} @ {file.Split('/').Last()}({line})");
			NSLog("|---------------------------------------------------------------------");
			NSLog($"|{obj?.ToString()}");
			NSLog("|---------------------------------------------------------------------");
		}

		public void E(object obj, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
		{
			NSLog("|---------------------------------------------------------------------");
			NSLog($"|{member} @ {file.Split('/').Last()}({line})");
			NSLog("|---------------------------------------------------------------------");
			NSLog($"|{obj?.ToString()}");
			NSLog("|---------------------------------------------------------------------");
		}
	}
}
