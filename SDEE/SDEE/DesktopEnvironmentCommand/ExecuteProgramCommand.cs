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

		public ExecuteProgramCommand(string executablePath)
		{
			ExecutablePath = executablePath;
		}

		public override void Execute()
		{
			using (Process pProcess = new Process())
			{
				pProcess.StartInfo.FileName = ExecutablePath;
				pProcess.Start();
			}
		}

		public override string ToString()
		{
			return $"Execute {ExecutablePath}";
		}
	}
}
