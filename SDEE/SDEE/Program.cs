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
using static Win32.User;
using static Win32.Kernel;
using System.Runtime.InteropServices;
using SFML.System;

namespace SDEE
{
	class Program
	{
		static void Main(string[] args)
		{
			//CustomDesktopEnvironment de = DesktopEnvironmentLoader.Load("config1");
			//CustomDesktopEnvironment de = new CustomDesktopEnvironment("config1");
			//de.Start();
			//MyDesktopEnvironment de = new MyDesktopEnvironment();

			// UnresolvedMergeConflict Cu

			using (var mde = new MyDesktopEnvironment())
			{
				mde.Start();
			}
		}
	}
}
