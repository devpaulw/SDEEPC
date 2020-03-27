using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDEE;
using SFML.Graphics;

namespace SDEE_Editor
{
	class Program
	{
		static void Main(string[] args)
		{
			new DesktopEnvironmentEditor().ReadCommands();
		}
	}
}
