using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;
using Win32;
using static Win32.User32;
using static Win32.Kernel;
using System.Runtime.InteropServices;
using SFML.System;

namespace SDEE
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				using (var mde = DesktopEnvironmentStorage.Load("de0"))
				{
					mde.Start();
				}
			}
			catch (Exception)
			{
				Console.WriteLine("Can't find or load the configuration...");
				Console.ReadLine();
			}
		}
	}
}
