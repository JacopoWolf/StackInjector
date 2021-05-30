using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet.Frameworks;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using StackInjector.Attributes;
using StackInjector.Settings;
using StackInjector.TEST.Structures.Simple;

namespace StackInjector.TEST.BlackBox
{
	[TestFixture]
	public class Serving
	{

#pragma warning disable 0649


		[Service]
		private class _NoServedAttribute
		{
			public IBase field;

			// for some fucked up reason if written as `public IBase property {get; set;}` is considered a field?!?!?
			public IBase property { get => this._base; set => this._base = value; }
			[Ignored]private IBase _base;
		}

		static IEnumerable<TestCaseData> NoAtt_ServingGenerator ()
		{
			// prop, field, servingm
			yield return new(Is.Null, Is.Null, StackWrapperSettings.ServeAllStrict);
			yield return new(Is.Not.Null, Is.Not.Null, StackWrapperSettings.ServeAll);
			yield return new(Is.Null, Is.Not.Null, ServingMethods.Fields);
			yield return new(Is.Not.Null, Is.Null, ServingMethods.Properties);
			yield return new(Is.Null, Is.Null, ServingMethods.None);
			yield return new(Is.Null, Is.Null, ServingMethods.Strict);
			yield break;
		}

		[Test]
		[TestCaseSource(nameof(NoAtt_ServingGenerator))]
		public void NoAttributes ( NullConstraint prop, NullConstraint field, ServingMethods serving )
		{
			var settings = StackWrapperSettings.Default;
			settings.Injection
					.ServingMethod(methods: serving, @override: true);

			var entry = Injector.From<_NoServedAttribute>(settings).Entry;


			Assert.Multiple(() =>
			{
				Assert.That(entry.property, prop, message: "property");
				Assert.That(entry.field, field, message: "field");
			});
		}


		[Service] private class _YesServedAttribute
		{
			[Served]
			public IBase field;

			[Served]
			public IBase property { get => this._base; set => this._base = value; }

			[Ignored]
			private IBase _base;
		}

		static IEnumerable<TestCaseData> Att_ServingGenerator ()
		{
			// prop, field, servingm
			yield return new(Is.Not.Null, Is.Not.Null, StackWrapperSettings.ServeAllStrict);
			yield return new(Is.Not.Null, Is.Not.Null, StackWrapperSettings.ServeAll);
			yield return new(Is.Null, Is.Not.Null, ServingMethods.Fields);
			yield return new(Is.Not.Null, Is.Null, ServingMethods.Properties);
			yield return new(Is.Null, Is.Null, ServingMethods.None);
			yield return new(Is.Null, Is.Null, ServingMethods.Strict);
			yield break;
		}

		[Test]
		[TestCaseSource(nameof(Att_ServingGenerator))]
		public void WithAtttributes ( NullConstraint prop, NullConstraint field, ServingMethods serving )
		{
			var settings = StackWrapperSettings.Default;
			settings.Injection
					.ServingMethod(methods: serving, @override: true);

			var entry = Injector.From<_YesServedAttribute>(settings).Entry;


			Assert.Multiple(() =>
			{
				Assert.That(entry.property, prop, message: "property");
				Assert.That(entry.field, field, message: "field");
			});
		}

	}
}
