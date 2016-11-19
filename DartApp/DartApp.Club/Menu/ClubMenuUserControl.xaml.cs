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

namespace DartApp.Club.Menu
{
	/// <summary>
	/// Interaction logic for ClubMenuUserControl.xaml
	/// </summary>
	public partial class ClubMenuUserControl : UserControl
	{
		public ClubMenuUserControl()
		{
			InitializeComponent();
			this.datagrid1.Loaded += Datagrid1_Loaded;
		}

		private void Datagrid1_Loaded(object sender, RoutedEventArgs e)
		{
			var vm = (ClubMenuViewModel) this.DataContext;
			if (vm != null)
			{
				var tournamentSeries = vm.SelectedSeries;
				this.datagrid1.Columns.Add(new DataGridTextColumn() {Header = "Name"});
				tournamentSeries.Tournaments.ForEach(x => this.datagrid1.Columns.Add(new DataGridTextColumn() {Header = x.Date.ToString("dd.MM.yyyy")}));
				this.datagrid1.Columns.Add(new DataGridTextColumn() { Header = "Gesamt" });
			}
		}
	}
}
