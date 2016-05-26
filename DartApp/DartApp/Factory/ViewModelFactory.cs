using DartApp.Database;
using DartApp.Home;
using DartApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DartApp.Factory
{
	public class ViewModelFactory : IViewModelFactory
	{
        private IEventService eventService;
		private static ViewModelFactory factory;
		private ViewModelFactory()
		{
            this.eventService = (IEventService)EventService.GetInstance();
		}

		public static ViewModelFactory GetInstance()
		{
			return factory ?? (factory = new ViewModelFactory());
		}

		public HomeViewModel GetHomeViewModel()
		{
			return new HomeViewModel(this.eventService);
		}


        public DatabaseMainViewModel GetDatabaseMainViewModel()
        {
            return new DatabaseMainViewModel(this.eventService);
        }
    }
}
