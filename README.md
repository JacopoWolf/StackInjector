<br/>
<p align="center">
<img src="logo/StackInjector_logo.svg" height="96" /> 
</p>

<h1 align="center">Stack Injector</h1>


<p align="center">
Modern, easy-to-use and fast dependency injection framework.<br><br>
<strong><a href="https://github.com/JacopoWolf/StackInjector/wiki">Documentation<a></strong>
<br>
<a href="https://www.nuget.org/packages/StackInjector">Download</a>
·
<a href="https://github.com/JacopoWolf/StackInjector/issues/new/choose">Report Bug</a>
·
<a href="https://github.com/JacopoWolf/StackInjector/issues/new/choose">Request Feature</a>
</p>


---


<p align=center>
<img src="https://img.shields.io/github/license/jacopowolf/stackinjector">
<img src="https://img.shields.io/maintenance/yes/2020">
<img src="https://img.shields.io/github/issues/jacopowolf/stackinjector/bug">
<img src="https://img.shields.io/nuget/dt/StackInjector?logo=nuget">
<br>
<img src="https://img.shields.io/badge/-Standard_2.1-5C2D91?logo=.net"/>
<img src="https://img.shields.io/badge/-8.0-239120?logo=c-sharp"/>
<img src="https://img.shields.io/nuget/vpre/StackInjector?label=">
</p>



---

Table of Contents
- [About](#about)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)
- [Acknowledgements](#acknowledgements)


## About

There are a lot of dependency injection frameworks for .NET, but every single one of them has some complicated specifics you have to learn (like active registration of components) and sometimes you don't really have full control of what to inject into your instances.

If you want to use an extremely easy dependency injection framework, and also use the latest features the C# language has to offer, then *StackInjector* is made for you!

*Also ships with some nice settings presets to suit your coding style!*


## Installation

```powershell
dotnet add package StackInjector
```

Or visit the [Nuget page](https://www.nuget.org/packages/StackInjector) for more options.



## Usage

In-depth tutorials and explanations can be found in the repository's [Wiki](https://github.com/JacopoWolf/StackInjector/wiki)

---

You can plan your components as **interfaces** and *implement* them! 

As clean as you can get!

```cs
interface IFooFilter
{
    string Filter( string element );
}
```

```cs
using StackInjector.Attributes;

[Service]
class SimpleFooFilter : IFooFilter
{
    // by default settings, you have to explicitly annotate with [Served]
    // the properties or fields you want injected

    [Served]
    IDatabaseAccess Database { get; set; }
    
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

Everything **must** be explicit (`[Service]`,`[Served]` and `[Ignored]` attributes) allowing for **extremely readable code** and for **granular control** over the injection process.

--- 

You then have multiple options on how to initialize your application, and every single one of them type-safe!

**synchronous**

a simple call to a static method and you're done!
```cs
// call DoWork() on a dependency-injected instance of IMyAppEntryPoint
Injector.From<IMyAppEntryPoint>().Entry.DoWork();
```

**asynchronous**

with C# 8's asynchronous enumerables waiting for task to complete is really this easy!

```cs
// the "using" keyword allows for safe disposing of the resources
using var app = Injector.AsyncFrom<IMyAsyncAppEntryPoint>( (e,i,t) => e.AsyncWork(i,t) );

// can be called from anywhere
app.Submit( "someInput" );

// waiting for completed tasks is this simple
await foreach ( var result in app.Elaborated() )
    Console.WriteLine( result );
```


## Contributing

Any contribution is appreciated! Especially bug reports, which mean you've been using this library I've been hard working on!

But first read [contributing](CONTRIBUTING.md) and [code of conduct](CODE_OF_CONDUCT.md) (I promise they're short)

*suggested editor: ![visualStudio](https://img.shields.io/badge/-Visual_Studio-5C2D91?logo=visual-studio)*



## License

Distributed under the `General Public Licence v3.0`.

See [LICENSE](LICENSE).


## Acknowledgements

- [Shields.io](https://shields.io/)
- [Readme-Template](https://github.com/othneildrew/Best-README-Template)

---
initial project and logo by [@JacopoWolf](https://github.com/JacopoWolf), 04/2020
