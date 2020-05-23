using System.Threading;
using System.Threading.Tasks;
using StackInjector.Wrappers;

namespace StackInjector.TEST.ComplexStack
{
	interface IRunBeforeStart
	{
		int Run ();
	}

	interface IBaseService : IStackEntryPoint
	{
		void Elaborate ();
	}

	interface IReadingService
	{
		Task<string> ReadAsync (CancellationToken token);
	}

	interface IAnsweringService
	{
		string ResponseTo ( string to );
	}


	// simple logging service with logging filter
	interface ILogger
	{
		void Log ( byte gravity, string message );
	}

	interface ILogFilter
	{
		bool LogLevel ( byte gravity );
	}

}