using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using StackInjector.Wrappers;

namespace StackInjector.TEST.ComplexStack
{
	interface IRunBeforeStart
	{
		int Run ();
	}

	interface IBaseService
	{
		object EntryPoint ();

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

	interface ITrickyEnumerable : IEnumerable<int>
	{
		void Trick ();
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