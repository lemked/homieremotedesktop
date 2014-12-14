using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Homie.Common;
using Homie.Common.WPF;
using Homie.Common.WPF.ProgressDialog;
using Homie.Model;
using MVVMLib.ViewModel;

namespace Homie.Admin.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
//        #region Fields
//
//        private static readonly object m_Lock = new object(); // required for cross-threaded access on ObservableCollections
//        private AddMachineWindow m_AddMachineWindow; // Reference to the "Add machine" dialog
//        private ObservableCollection<Machine> m_MachineCollection;
//        private ObservableCollection<EventLogEntry> m_EventLogEntries = new ObservableCollection<EventLogEntry>();
//
//        #endregion
//
//        #region Properties
//        /// <summary>
//        /// 
//        /// </summary>
//        public ObservableCollection<Machine> MachineCollection
//        {
//            get
//            {
//                if (m_MachineCollection == null)
//                {
//                    m_MachineCollection = new ObservableCollection<Machine>();
//                }
//                return m_MachineCollection;
//            }
//        }
//
//        public ServiceControllerStatus ServiceStatus
//        {
//            get
//            {
//                ServiceController lServiceController = new ServiceController(Constants.cServiceName);
//                lServiceController.Refresh(); // update properties of pServiceController
//
//                // If service not running, check if the process exists as a non-service application.
//                // This is the case for example when the service was started with /NOSERVICE.
//                if (lServiceController.Status != ServiceControllerStatus.Running)
//                {
//                    if (Process.GetProcessesByName(Constants.cServiceExeName).Length != 0)
//                    {
//                        return ServiceControllerStatus.Running;
//                    }
//                }
//
//                return lServiceController.Status;
//            }
//        }
//
//        public ObservableCollection<EventLogEntry> EventLogEntries
//        {
//            get { return m_EventLogEntries; }
//            set { m_EventLogEntries = value; }
//        }
//
//        private static EventLogEntry LastEventLogEntry
//        {
//            get
//            {
//                EventLog lEventLog = new EventLog(Constants.cServiceDisplayName);
//                if (lEventLog.Entries.Count > 0)
//                {
//                    return lEventLog.Entries[lEventLog.Entries.Count - 1];
//                }
//                return null;
//            }
//        }
//
//        #endregion
//
//        private void PopulateMachineList()
//        {
//            try
//            {
//                List<Machine> lMachines = ServiceCall.GetAllMachines();
//                foreach (var lMachine in lMachines)
//                {
//                    this.MachineCollection.Add(lMachine);
//                }
//            }
//            catch (Exception lException)
//            {
//                Log.Exception(lException);
//            }
//        }
//
//        private void RefreshMachineList()
//        {
//            this.MachineCollection.Clear();
//            PopulateMachineList();
//        }

        public MainWindow()
        {
            InitializeComponent();
//
//            // Enable the cross access to the EventLog collection (required for non-UI threads)
//            BindingOperations.EnableCollectionSynchronization(m_EventLogEntries, m_Lock);
//
//            // Add the required bindings for sorting the WPF ListView
//            AddSortBinding();
//
//            // Provide the data for the EventLog control. A new task is started here to 
//            // avoid any delays that may be caused by processing the EventLog.
//            Task.Factory.StartNew(RetrieveEventLogData);
//
//            // When the entries have been added, apply an event handler so we are notified 
//            // about new entries. This way we don't have to use a time or so that reparses
//            // The EventLog from in constantly.
//            SubscribeToEventLog(Constants.cServiceDisplayName);
//
//            // Set the controls corresponding to the Service status
//            RefreshControls(ServiceStatus);
//
//            // If service is already running, retrieve the machines from it
//            if (ServiceStatus == ServiceControllerStatus.Running)
//            {
//                PopulateMachineList();
//            }
//
//            // Create a tray icon for this application
//            System.Windows.Forms.NotifyIcon lNotifyIcon = new System.Windows.Forms.NotifyIcon();
//            //lNotifyIcon.Icon = Common.Properties.Resources;
//            lNotifyIcon.Visible = true;
//            lNotifyIcon.DoubleClick += NotifyIconOnDoubleClick;
        }

//        void NotifyIconOnDoubleClick(object pSender, EventArgs pEventArgs)
//        {
//            this.Show();
//            this.WindowState = WindowState.Normal;
//        }
//
//
//        private void AddSortBinding()
//        {
//            GridView lGridView = (GridView)EventLogListView.View;
//
//            //ListViewSorter.SetSortBindingMember(lGridView.Columns[0], new Binding("Message"));
//            ListViewSorter.SetSortBindingMember(lGridView.Columns[1], new Binding(EventLogUtils.cDateTimeColumnName));
//            ListViewSorter.SetSortBindingMember(lGridView.Columns[2], new Binding(EventLogUtils.cMessageColumnName));
//
//            ListViewSorter.SetCustomSorter(EventLogListView, new EventLogSorter());
//        }
//
//        #region EventLog related methods
//
//        private void RetrieveEventLogData()
//        {
//            m_EventLogEntries.Clear();
//
//            // Retrieve the entries within the specified time span out of the EventLog and 
//            // provide them in the ObersableCollection that is used by the XAML ListView
//            DateTime lLastYear = DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0)); //TODO: make TimeSpan configurable
//            List<EventLogEntry> lEventLogEntries = EventLogUtils.GetEventLogEntriesSince(Constants.cServiceDisplayName, lLastYear, EventLogEntryType.Information);
//            foreach (var lEventLogEntry in lEventLogEntries)
//            {
//                m_EventLogEntries.Add(lEventLogEntry);
//            }
//        }
//
//        /// <summary>
//        /// Subscribes the application to the given EventLog
//        /// Idea from: http://msdn.microsoft.com/en-us/library/bb671202%28v=vs.90%29.aspx
//        /// </summary>
//        public void SubscribeToEventLog(string pEventLogName)
//        {
//            // Subscribe to receive event notifications in the Application log
//            EventLogQuery lSubscriptionQuery = new EventLogQuery(pEventLogName, PathType.LogName);
//            EventLogWatcher lWatcher = new EventLogWatcher(lSubscriptionQuery);
//
//            // Set watcher to listen for the EventRecordWritten
//            // event.  When this event happens, the callback method
//            // (EventLogEventRead) will be called.
//            lWatcher.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(EventLogEventRead);
//
//            // Begin subscribing to events the events
//            lWatcher.Enabled = true;
//        }
//
//        /// <summary>
//        /// This method retrieves the last entry from the EventLog and adds it to the collection 
//        /// that is associated with the XAML Listview that shows the log messages.
//        /// </summary>
//        private void AppendLastEventLogEntryToCollection()
//        {
//            EventLogEntry lLastEventLogEntry = LastEventLogEntry;
//            if (lLastEventLogEntry != null)
//            {
//                m_EventLogEntries.Add(lLastEventLogEntry);
//            }
//        }
//
//        /// <summary>
//        /// Callback method that gets executed when an event is reported to the subscription.
//        /// </summary>
//        private void EventLogEventRead(object pObject, EventRecordWrittenEventArgs pArg)
//        {
//            // A new task is started here to avoid any delays that may be caused
//            // by processing the EventLog
//            Task.Factory.StartNew(AppendLastEventLogEntryToCollection);
//        }
//
//        #endregion
//
//        private void RefreshControls(ServiceControllerStatus pServiceStatus)
//        {
//            if (pServiceStatus == ServiceControllerStatus.Stopped)
//            {
//                StartServiceButton.IsEnabled = true;
//                StopServiceButton.IsEnabled = false;
//                ServiceStatusLabel.Text = "Stopped";
//            }
//            if (pServiceStatus == ServiceControllerStatus.Running)
//            {
//                StartServiceButton.IsEnabled = false;
//                StopServiceButton.IsEnabled = true;
//                ServiceStatusLabel.Text = "Running";
//            }
//        }
//
//        private void StartServiceButtonClick(object pSender, RoutedEventArgs pRoutedEventArgs)
//        {
//            var lServiceController = new ServiceController(Constants.cServiceName);
//            var lMachineCollection = new ObservableCollection<Machine>();
//            ProgressDialogResult lResult = ProgressDialog.Execute
//            (
//                this, Common.Properties.Resources.ServiceStarting, () =>
//                {
//                    lServiceController.Start();
//                    lServiceController.Refresh();
//                    while (lServiceController.Status == ServiceControllerStatus.StartPending)
//                    {
//                        Thread.Sleep(100);
//                        lServiceController.Refresh();
//                    }
//                    
//                    // Retrieve the machines from the service
//                    PopulateMachineList();
//                }
//            );
//
//            if (lResult.OperationFailed)
//            {
//                // TODO: Notify if starting has failed                
//            }
//            else
//            {
//                RefreshControls(ServiceControllerStatus.Running);
//                lServiceController.Refresh();
//                m_MachineCollection = lMachineCollection;
//            }
//        }
//
//        private void StopServiceButtonClick(object pSender, RoutedEventArgs pRoutedEventArgs)
//        {
//            var lServiceController = new ServiceController(Constants.cServiceName);
//            ProgressDialogResult lResult = ProgressDialog.Execute(
//                this, Common.Properties.Resources.ServiceStopping, () =>
//                {
//                    lServiceController.Stop();
//                    lServiceController.Refresh();
//                    while (lServiceController.Status == ServiceControllerStatus.StopPending)
//                    {
//                        Thread.Sleep(100);
//                        lServiceController.Refresh();
//                    }
//                }
//            );
//
//            if (lResult.OperationFailed)
//            {
//                // TODO: Notify if stopping has failed                
//            }
//            else
//            {
//                RefreshControls(ServiceControllerStatus.Stopped);
//                lServiceController.Refresh();
//                lServiceController.Refresh();
//            }
//        }
//
//        private void AddMachineButtonClick(object pSender, RoutedEventArgs pRoutedEventArgs)
//        {
//            if (ServiceStatus != ServiceControllerStatus.Running)
//            {
//                StartServiceButtonClick(pSender, pRoutedEventArgs);
//            }
//
//            m_AddMachineWindow = new AddMachineWindow();
//            m_AddMachineWindow.ShowDialog();
//
//            // Retrieve the machines from the service
//            RefreshMachineList();
//        }
//
//        private void RemoveMachineButtonClick(object pSender, RoutedEventArgs pRoutedEventArgs)
//        {
//            if (ServiceStatus != ServiceControllerStatus.Running)
//            {
//                StartServiceButtonClick(pSender, pRoutedEventArgs);
//            }
//
//            Machine lSelectedMachine = MachinesListView.SelectedItem as Machine;
//            if (lSelectedMachine == null) return;
//            // Call the service to remove the machine.
//            if (ServiceCall.RemoveMachine(lSelectedMachine))
//            {
//                // If this was successfull, remove it from the ListView as well.
//                m_MachineCollection.Remove(lSelectedMachine);
//            }
//        }
//
//        private void MachinesListViewMouseDoubleClick(object pSender, System.Windows.Input.MouseButtonEventArgs pMouseButtonEventArgs)
//        {
//            Machine lSelectedMachine = MachinesListView.SelectedItem as Machine;
//            m_AddMachineWindow = new AddMachineWindow(lSelectedMachine);
//            m_AddMachineWindow.ShowDialog();
//            RefreshMachineList();
//        }
    }
}