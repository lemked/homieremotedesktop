using System;
using System.Collections.Generic;
using Homie.Client.View;
using Homie.Client.ViewModel;
using MVVMLib.WindowViewModelMapping;

namespace Homie.Client.WindowViewModelMapping
{
	/// <summary>
	/// Class describing the Window-ViewModel mappings.
	/// </summary>
	public class WindowViewModelMappings : WindowViewModelMappingBase, IWindowViewModelMappings
	{
	    private static Dictionary<Type, Type> mapping = new Dictionary<Type, Type>
	    {
            // Mapping for the main window
	        //{typeof (MainWindowViewModel), typeof (string)},
            // Add other required mappings here
	        {typeof (OptionsWindowViewModel), typeof (OptionsWindow)},
            {typeof (ConnectWindowViewModel), typeof(ConnectWindow)}
	    };

		/// <summary>
		/// Initializes a new instance of the <see cref="WindowViewModelMappings"/> class.
		/// </summary>
        public WindowViewModelMappings() : base(mapping)
		{

		}
	}
}