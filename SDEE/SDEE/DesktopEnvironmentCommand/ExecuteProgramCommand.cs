using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
	class ExecuteProgramCommand : DesktopEnvironmentCommand
	{
		public string ExecutablePath { get; private set; }

		public ExecuteProgramCommand(DesktopEnvironment desktopEnvironment, string executablePath) : base(desktopEnvironment)
		{
			ExecutablePath = executablePath;
		}

		public override void Execute()
		{
			DesktopEnvironment.StartExe(ExecutablePath);
		}

		public override string ToString()
		{
			return $"Executing {ExecutablePath}...";
		}
	}
}
