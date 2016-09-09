using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartApp.Services
{
    public interface IEventService
    {
        void PublishDisplayChangedEvent(DisplayEnum displayEnum);
		void PublishDisplayChangedEvent(DisplayEnum displayEnum, List<object> eventArgs);
    }
}
