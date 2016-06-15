using DartApp.Database;
using DartApp.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartApp.Factory
{
    public interface IViewModelFactory
    {
		MainViewModel GetMainViewModel();
        HomeViewModel GetHomeViewModel();
        DatabaseMainViewModel GetDatabaseMainViewModel();
    }
}
