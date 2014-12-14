using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Threading.Tasks;

namespace Homie.Service.EventLog
{
    public class EventLogProvider : IEventLogProvider
    {
        public async Task<List<EventLogEntry>> GetEventLogEntriesAsync(string pEventLogName, DateTime pWrittenSince)
        {
            return await Task.Factory.StartNew(() => GetEventLogEntries(pEventLogName, pWrittenSince));
        }

        private List<EventLogEntry> GetEventLogEntries(string pEventLogName, DateTime pWrittenSince)
        {
            var lResults = new List<EventLogEntry>();
            var lEventLog = new System.Diagnostics.EventLog(pEventLogName);
            foreach (EventLogEntry lEntry in lEventLog.Entries)
            {
                if (lEntry.TimeWritten > pWrittenSince)
                {
                    lResults.Add(lEntry);
                }
            }

            return lResults;
        }

        public void Subscribe(string pEventLogName)
        {
            // Subscribe to receive event notifications in the Application log
            var lSubscriptionQuery = new EventLogQuery(pEventLogName, PathType.LogName);
            var lWatcher = new EventLogWatcher(lSubscriptionQuery);

            // Set watcher to listen for the EventRecordWritten
            // event.  When this event happens, the callback method
            // (EventLogEventRead) will be called.
            lWatcher.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(EventLogEventRead);

            // Begin subscribing to events the events
            lWatcher.Enabled = true;
        }

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

        /// <summary>
        /// Callback method that gets executed when an event is reported to the subscription.
        /// </summary>
        private void EventLogEventRead(object pObject, EventRecordWrittenEventArgs pArg)
        {
//            var lEventLog = new System.Diagnostics.EventLog(Constants.cServiceDisplayName);
//            if (lEventLog.Entries.Count > 0)
//            {
//                return lEventLog.Entries[lEventLog.Entries.Count - 1];
//            }
//
//            EventLogEntry lLastEventLogEntry = LastEventLogEntry;
//            if (lLastEventLogEntry != null)
//            {
//                m_EventLogEntries.Add(lLastEventLogEntry);
//            }
        }

        public event EventHandler NewEntryAdded;
    }
}
