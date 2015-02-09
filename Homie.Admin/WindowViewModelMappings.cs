using System;
using System.Collections.Generic;

using Homie.Admin.View;
using Homie.Admin.View.Machine;
using Homie.Admin.View.User;
using Homie.Admin.ViewModel;

using MVVMLib.WindowViewModelMapping;

namespace Homie.Admin
{
	/// <summary>
	/// Class describing the Window-ViewModel mappings.
	/// </summary>
	public class WindowViewModelMappings : WindowViewModelMappingBase
	{
	    private static readonly Dictionary<Type, Type> mapping = new Dictionary<Type, Type>
	    {
            // Mapping for the main window
            {typeof (EventLogViewModel), typeof (EventLogView)},
            {typeof (MachinesViewModel), typeof (MachinesView)},
            {typeof (UsersViewModel), typeof (UsersView)},
            {typeof (AddMachineWindowViewModel), typeof (AddMachineWindow)},
            {typeof (EditMachineWindowViewModel), typeof (EditMachineWindow)},
            {typeof (AddUserWindowViewModel), typeof (AddUserWindow)},
            {typeof (EditUserWindowViewModel), typeof (EditUserWindow)}
	    };

		/// <summary>
		/// Initializes a new instance of the <see cref="WindowViewModelMappings"/> class.
		/// </summary>
        public WindowViewModelMappings() : base(mapping)
		{

		}
	}
}