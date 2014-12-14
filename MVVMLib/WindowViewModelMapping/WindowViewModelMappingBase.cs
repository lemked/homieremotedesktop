using System;
using System.Collections.Generic;

namespace MVVMLib.WindowViewModelMapping
{
	/// <summary>
	/// Class describing the Window-ViewModel mappings used by the dialog service.
	/// </summary>
	public class WindowViewModelMappingBase : IWindowViewModelMappings
	{
		private IDictionary<Type, Type> mappings;

		/// <summary>
		/// Initializes a new instance of the <see cref="WindowViewModelMappingBase"/> class.
		/// </summary>
		public WindowViewModelMappingBase(Dictionary<Type, Type> pMappings)
		{
		    this.mappings = pMappings;
		}

		/// <summary>
		/// Gets the window type based on registered ViewModel type.
		/// </summary>
		/// <param name="viewModelType">The type of the ViewModel.</param>
		/// <returns>The window type based on registered ViewModel type.</returns>
		public Type GetWindowTypeFromViewModelType(Type viewModelType)
		{
            // Resolves the ViewModel into a View. This will fail if there wasn't
            // a mapping defined for the given ViewModel type. Ensure to add all
            // required mappings to your WindowViewModelMappingBase descendant.
            return mappings[viewModelType];
		}
	}
}