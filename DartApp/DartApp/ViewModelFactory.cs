using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DartApp
{
	public class ViewModelFactory
	{
		private static ViewModelFactory factory;
		private ViewModelFactory()
		{
		}

		public static ViewModelFactory GetInstance()
		{
			return factory ?? (factory = new ViewModelFactory());
		}

		public HomeViewModel GetHomeViewModel()
		{
			return new HomeViewModel();
		}
	}
}
