![dotNetStandard2.1](https://img.shields.io/badge/-Standard_2.1-5C2D91?logo=.net&style=flat-square) 
![csharp8.0](https://img.shields.io/badge/-8.0-239120?logo=c-sharp&style=flat-square)
![GitHub](https://img.shields.io/github/license/jacopowolf/stackinjector?style=flat-square)
![Maintenance](https://img.shields.io/maintenance/yes/2020?style=flat-square)
![GitHub open bug issues](https://img.shields.io/github/issues/jacopowolf/stackinjector/bug?style=flat-square)
[![Nuget](https://img.shields.io/nuget/vpre/StackInjector?logo=nuget&style=flat-square)](https://www.nuget.org/packages/StackInjector)
[![Nuget](https://img.shields.io/nuget/dt/StackInjector?logo=nuget&style=flat-square)](https://www.nuget.org/packages/StackInjector)


<br/>
<p align="center">

<img src="logo/StackInjector_logo.svg" height="128" /> 
<h1 align="center">Stack Injector</h1>

<p align="center">
Simple, fast and easy-to-use dependency injection
<br/>
<strong><a href="https://github.com/JacopoWolf/StackInjector/wiki">Documentation »<a></strong>
<br>
<br>
<a href="https://www.nuget.org/packages/StackInjector">Download</a>
·
<a href="https://github.com/JacopoWolf/StackInjector/issues/new/choose">Report Bug</a>
·
<a href="https://github.com/JacopoWolf/StackInjector/issues/new/choose">Request Feature</a>
</p>

</p>


---

- [About](#about)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)
- [Acknowledgements](#acknowledgements)


## About

There are a lot of dependency injection frameworks for .NET, but every single one of them has some complicated specifics you have to learn (like active registration of components) and sometimes you don't really have full control of what to inject into your instances.

If you want to use an extremely easy dependency injection framework, then *StackInjector* is made for you! 


## Installation

```powershell
dotnet add package StackInjector
```

Or visit the [Nuget page](https://www.nuget.org/packages/StackInjector) for more options.



## Usage

In-depth tutorials and explanations can be found in the [wiki](https://github.com/JacopoWolf/StackInjector/wiki)

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

// you can optionally specify a version for your service implementation!
[Service(Version=1.0)]
class SimpleFooFilter : IFooFilter
{
    // both fields and properties, to explicitly annotate with [Served]
    // if you want them injected

    [Served]
    IDatabaseAccess Database { get; set; }
    
    // works with properties too!
    [Served]
    IFooFilter filter;

    
    string Filter( string element ) 
    {
        // those are just random examples! 
        var item = this.Database.SomeMethod( element );
        this.filter.Filter( item );
        return element;
    }
}
```

Everything **must** have an attribute, allowing for **extremely readable code** and for **granular control** over the injection process.

--- 

You then have multiple options on how you want to initialize your application, and every single one of them type-safe!

```cs
using StackInjector;
```

**synchronous**
```cs
Injector.From<IMyAppEntryPoint>().Start( app => app.Work() );
```
or 
```cs
Injector.From<IMyAppEntryPoint>().Entry.Work();
```

**asynchronous**
```cs
// the using keyword allows for safe disposing of the resources
using var app = Injector.AsyncFrom<MyAsyncAppEntryPoint>( (e,i,t) => e.Elaborate(i,t) );

// can be called from anywhere and guarantee consistency
app.Submit( someInput );

// waiting for completed tasks is this simple
await foreach ( var result in app.Elaborated() )
    Console.WriteLine( result );
```



## Contributing

Any contribution is appreciated! Especially bug reports, which mean you've been using this library I've been hard working on!

But first read [contributing](CONTRIBUTING.md) and [code of conduct](CODE_OF_CONDUCT.md) (I promise they're short)

*suggested editor: ![visualStudio](https://img.shields.io/badge/-Visual_Studio-5C2D91?logo=visual-studio&style=flat-square)*



## License

Distributed under the `General Public Licence v3.0`.

See [LICENSE](LICENSE).


## Acknowledgements

- [Shields.io](https://shields.io/)
- [Readme-Template](https://github.com/othneildrew/Best-README-Template)

---
initial project and logo by [@JacopoWolf](https://github.com/JacopoWolf), 04/2020
