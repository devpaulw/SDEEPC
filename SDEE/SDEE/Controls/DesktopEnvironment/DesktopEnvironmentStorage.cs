using System;
using System.IO;

namespace SDEE
{
	public static class DesktopEnvironmentStorage
	{
		public const char XmlAttributeSeparator = '-';
		public static string ConfigurationDirectory { get => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SDEE"); }
	}
}