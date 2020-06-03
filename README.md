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

// you can optionally specify a version for your service implementation!
[Service(Version=1.0)]
class SimpleFooFilter : IFooFilter
{
    // both fields and properties, to explicitly annotate with [Served]
    // if you want them injected

    [Served]
    IDatabaseAccess database { get; set; }
    
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

You then have multiple options on how you want to initialize your application! 

```cs
using StackInjector;
```

**synchronous**
```cs
Injector.From<IMyAppEntryPoint>().Start();
```

**asynchronous**
```cs
// using allows for safe disposing of the resources when no more needed
using var app = Injector.AsyncFrom<MyAsyncAppEntryPoint>();

// can be called from anywhere and guarantee consistency
app.Submit( someInput );

// waiting for completed tasks is as simple as an await foreach loop
await foreach ( var result in app.Elaborated<string>() )
    Console.WriteLine( result );
```

**generics**

For users who do care about type safety, there are also generic options! Both synchronous and asynchronous.

```cs
// generic asynchronous wrapper
using var app = 
    Injector.AsyncFrom<MyAsyncAppEntryPoint,string,int>
        (
            async (entryPoint,element,cancellationToken)
                => await entryPoint.MyCustomLogic(element)
        );
```

---

For **more** information and in-depth **tutorials**, look at the simple tutorials in the [wiki](https://github.com/JacopoWolf/StackInjector/wiki)



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
