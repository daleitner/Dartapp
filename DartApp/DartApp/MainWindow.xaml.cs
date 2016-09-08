using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using DartApp.CommandServices;
using DartApp.Factory;
using DataBaseInitializer;
using System.IO;

namespace DartApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			var setup = Directory.GetCurrentDirectory() + "\\database.xml";
			var testValueFile = Directory.GetCurrentDirectory() + "\\dbtestvalues.txt";
			var mappingPath = Directory.GetCurrentDirectory() + "\\mapping.xml";

			try
			{
				var dbCreator = DataBaseManager.GetInstance(setup, mappingPath, testValueFile);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			var viewModel = ViewModelFactory.GetInstance().GetMainViewModel();
			this.DataContext = viewModel;
			InitializeComponent();
		}
	}
}
