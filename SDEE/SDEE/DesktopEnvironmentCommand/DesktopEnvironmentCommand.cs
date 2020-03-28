using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
	abstract class DesktopEnvironmentCommand
	{
		public DesktopEnvironment DesktopEnvironment { get; set; }

		protected DesktopEnvironmentCommand(DesktopEnvironment desktopEnvironment)
		{
			DesktopEnvironment = desktopEnvironment;
		}

		abstract public void Execute();
	}
}
