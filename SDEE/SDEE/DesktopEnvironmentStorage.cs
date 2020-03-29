using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SDEE
{
	public static class DesktopEnvironmentStorage
	{
		public static string ConfigurationDirectory { get => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SDEE"); }
	}
}
