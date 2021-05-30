using StackInjector.Attributes;

namespace StackInjector.Settings
{
	public partial class StackWrapperSettings
	{
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
