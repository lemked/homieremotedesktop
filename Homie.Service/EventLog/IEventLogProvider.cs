using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Homie.Service.EventLog
{
    public interface IEventLogProvider
    {
        Task<List<EventLogEntry>> GetEventLogEntriesAsync(string pEventLogName, DateTime pWrittenSince);

        /// <summary>
        /// Subscribes the event log provider to the given log. The NewEntryAdded event will be fired once a new entry is added.
        /// </summary>
        /// <author>Daniel Lemke - lemked@web.de</author>
//        void Subscribe(string pEventLogName);
//
//        event EventHandler NewEntryAdded;
    }
}
