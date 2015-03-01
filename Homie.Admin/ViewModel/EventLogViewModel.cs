using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
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

        public ICommand LoadEntriesCommand
        {
            get
            {
                return new RelayCommand(action => GetEventLogEntriesAsync());
            }
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
