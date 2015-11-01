using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homie.Client.ConnectionManagement
{
    interface IConnectionHandler
    {
        event EventHandler ConnectionInitiated;

        event EventHandler ConnectionClosed;

        event EventHandler<StatusChangedEventArgs> StatusChanged;
    }
}
