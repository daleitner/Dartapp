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
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{ 
			System.Windows.Controls.PrintDialog Printdlg = new System.Windows.Controls.PrintDialog();
			if ((bool)Printdlg.ShowDialog().GetValueOrDefault())
			{
				//Size pageSize = new Size(Printdlg.PrintableAreaWidth, Printdlg.PrintableAreaHeight);
				// sizing of the element.
				//datagrid1.Measure(pageSize);
				//datagrid1.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
				Printdlg.PrintVisual(grid1, "TEST");
			}
		
		}
	}
}
