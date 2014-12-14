using System;
using System.Collections.Generic;

using Homie.Admin.View;
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
            {typeof (AddMachineWindowViewModel), typeof (AddMachineWindow)},
            {typeof (EditMachineWindowViewModel), typeof (EditMachineWindow)}
	    };

		/// <summary>
		/// Initializes a new instance of the <see cref="WindowViewModelMappings"/> class.
		/// </summary>
        public WindowViewModelMappings() : base(mapping)
		{

		}
	}
}