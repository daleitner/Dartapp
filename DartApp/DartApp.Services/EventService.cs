using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartApp.Services
{
    public class EventService : IEventService
    {
        public delegate void DisplayChangedEventHandler(DisplayEnum displayEnum, List<object> eventArgs);
        public event DisplayChangedEventHandler DisplayChanged = null;

        #region singelton
        private static EventService instance;
        private EventService()
        {
        }

        public static EventService GetInstance()
        {
            if (instance == null)
                instance = new EventService();
            return instance;
        }
        #endregion

        public void PublishDisplayChangedEvent(DisplayEnum displayEnum)
        {
            if (this.DisplayChanged != null)
                this.DisplayChanged(displayEnum, null);
        }



		public void PublishDisplayChangedEvent(DisplayEnum displayEnum, List<object> eventArgs)
		{
			if (this.DisplayChanged != null)
				this.DisplayChanged(displayEnum, eventArgs);
		}
	}
}
