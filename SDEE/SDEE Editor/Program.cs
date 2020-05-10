using SDEE.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
	class Program
	{
		static void Main(string[] args)
		{
			DesktopEnvironmentStorer.Save(new DesktopEnvironment());
		}
	}
}
