using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xaml.Permissions;

namespace SDEE.Framework
{
	public static class DesktopEnvironmentStorer
	{
		public static string sdeePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SDEE");


		public static void Save(string configurationName, DesktopEnvironment desktopEnvironment)
		{
			Directory.CreateDirectory(sdeePath);
			FileStream fileStream = File.Open(Path.Combine(sdeePath, $"{configurationName}.xaml"), FileMode.Create);
			XamlWriter.Save(desktopEnvironment.FloatingElements[0], fileStream);
			fileStream.Close();
		}

		public static DesktopEnvironment Load(string configurationName)
		{
			var de = new DesktopEnvironment();
			FileStream fileStream = File.Open(Path.Combine(sdeePath, $"{configurationName}.xaml"), FileMode.Open);
			Grid grid = (Grid)XamlReader.Load(fileStream);
			fileStream.Close();

			de.FloatingElements.Add(grid);
			return de;
		}
	}
}
