using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SDEE_Editor
{
	/// <summary>
	/// Interaction logic for Toolbox.xaml
	/// </summary>
	public partial class Toolbox : UserControl
	{
		public Toolbox()
		{
			InitializeComponent();
			Initialize();
		}

		public void Initialize()
		{
			string sdeePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SDEE", "frameworks");
			string[] directories = Directory.GetDirectories(sdeePath);

			foreach (var directory in directories)
			{
				string[] files = Directory.GetFiles(System.IO.Path.Combine(sdeePath, directory));
				var group = new TreeViewItem
				{
					Header = System.IO.Path.GetFileName(directory)
				};

				foreach (var file in files)
				{
					var item = new TreeViewItem()
					{
						Header = System.IO.Path.GetFileNameWithoutExtension(file)
					};
					group.Items.Add(item);
				}
				_tree.Items.Add(group);
			}
		}
	}
}
