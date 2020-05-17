<h1> 
    <img src="logo/StackInjector_logo_notext.svg" height="35px" /> 
    StackInjector
</h1>


![dotNetStandard2.1](https://img.shields.io/badge/-Standard_2.1-5C2D91?logo=.net) 
![csharp8.0](https://img.shields.io/badge/-8.0-239120?logo=c-sharp)

A **simple**, **performing**, **easy-to-use** dependency injection library for you stack-structured applications, like custom servers, interpreters, etc.

![Maintenance](https://img.shields.io/maintenance/yes/2020)
![GitHub open bug issues](https://img.shields.io/github/issues/jacopowolf/stackinjector/bug)

Visit the [Wiki](https://github.com/JacopoWolf/StackInjector/wiki) for more information!



## Installation :electric_plug:

[![Nuget](https://img.shields.io/nuget/vpre/StackInjector?logo=nuget)](https://www.nuget.org/packages/StackInjector/)
[![Nuget](https://img.shields.io/nuget/dt/StackInjector?logo=nuget)](https://www.nuget.org/packages/StackInjector/)

```powershell
dotnet add package StackInjector
```
Or visit the [Nuget page](https://www.nuget.org/packages/StackInjector)



## Usage :mortar_board:

In-depth tutorials and explanations can be found in the [wiki tutorials section](https://github.com/JacopoWolf/StackInjector/wiki/Tutorial_Introduction)

---

Plan your components as **interfaces** and *implement* them! 

As clean as you can get!

```cs
interface IFooFilter
{
    string Filter( string element );
}
```

```cs
using StackInjector.Attributes;

// you can specify a version for your service implementation!
[Service(Version=1.0)]
class SimpleFooFilter : IFooFilter
{
    // both fields and properties, to explicitly annotate with [Served]

    [Served]
    IDatabaseAccess database { get; set; }
    
    // you can have multiple implementations and require a specific one 
    // either by requesting it's version or by specifying the class

    [Served(TargetVersion=2.0)]
    IFooFilter advancedFilter { get; set; }

    [Served]
    AnotherFooFilter anotherFilter;

    
    string Filter( string element ) 
    {
        // those are just random examples! 
        var item = this.Database.SomeMethod( element );
        this.advancedFilter.Filter( anotherFilter.Filter(item) );
        return element;
    }
}
```

Everything **must** have an attribute, allowing for **extremely readable code** and for **granular control** over the injection process.

--- 

You then have multiple options on how you want to initialize your application! 

Remember that you can initialize a new wrapper on-the-fly anywhere in your application!

```cs
using StackInjector;
```

**synchronous**
```cs
Injector.From<IMyAppEntryPoint>().Start();
```

**asynchronous**
```cs
using var App = Injector.AsyncFrom<MyAsyncAppEntryPoint>();

// atomic call, can be called from anywhere and guarantee consistency
App.Submit( someInput );

// waiting for completed tasks is as simple as an await foreach loop
await foreach ( var result in App.Elaborated<string>() )
    Console.WriteLine( result );
```

**asynchronous with custom logic**

```cs
using StackInjector.Generics;
```

```cs
using var App = 
    Injector.AsyncFrom<MyAsyncAppEntryPoint,string,int>
        (
            async (entryPoint,element,cancellationToken)
                => await entryPoint.MyCustomLogic(element)
        );
```

---

For **more** information and in-depth **tutorials**, look at the simple tutorials in the [wiki](https://github.com/JacopoWolf/StackInjector/wiki)



## Contributing :pencil2:

Any contribution is appreciated! Thanks!

But first read [contributing](CONTRIBUTING.md) and [code of conduct](CODE_OF_CONDUCT.md)

suggested editor: ![visualStudio](https://img.shields.io/badge/-Visual_Studio-5C2D91?logo=visual-studio)



## License :scroll:

![GitHub](https://img.shields.io/github/license/jacopowolf/stackinjector)

---
initial project and logo by [@JacopoWolf](https://github.com/JacopoWolf) as of 04/2020
