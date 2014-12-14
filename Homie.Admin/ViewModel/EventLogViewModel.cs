using System.Collections.ObjectModel;
using System.Threading.Tasks;

using MVVMLib.ViewModel;

using Homie.Common.Interfaces;
using Homie.Model.Logging;

namespace Homie.Admin.ViewModel
{
    public class EventLogViewModel : ViewModelBase
    {
        private readonly IServiceLogProvider serviceLogProvider;

        private readonly ObservableCollection<LogMessage> eventLogEntries = new ObservableCollection<LogMessage>();
        public ObservableCollection<LogMessage> EventLogEntries
        {
            get { return eventLogEntries; }
        }
        public EventLogViewModel(IServiceLogProvider serviceLogProvider)
        {
            this.serviceLogProvider = serviceLogProvider;
        }

        public async Task GetEventLogEntriesAsync()
        {
            this.eventLogEntries.Clear();

            foreach (var eventLogEntry in await serviceLogProvider.GetLogEntriesAsync())
            {
                this.eventLogEntries.Add(eventLogEntry);
            }
        }
    }
}
