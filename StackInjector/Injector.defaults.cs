using StackInjector.Attributes;
using StackInjector.Settings;

namespace StackInjector
{
	public static partial class Injector
	{
		/// <summary>
		/// Static class for default settings.
		/// </summary>
		public static class Defaults
		{
			// the const keyword allows usage in attributes

			/// <summary>
			/// The default serving method
			/// </summary>
			public const ServingMethods ServeAllStrict =
				( ServingMethods.Fields | ServingMethods.Properties | ServingMethods.Strict);

			/// <summary>
			/// Serve every field and property ignoring if they have the <see cref="ServedAttribute"/>
			/// </summary>
			public const ServingMethods ServeAll =
				( ServingMethods.Fields | ServingMethods.Properties );


			/// <summary>
			/// Default regex to filter system assemblies
			/// </summary>
			public const string AssemblyRegexFilter = "^(System)|(Microsoft)";
		}
	}
}
