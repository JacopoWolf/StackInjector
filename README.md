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
<img src="https://img.shields.io/nuget/dt/StackInjector?logo=nuget">
<img src="https://img.shields.io/nuget/vpre/StackInjector?label=">
<br>
<img src="https://img.shields.io/maintenance/no/2021">
<br>
<img src="https://img.shields.io/github/license/jacopowolf/stackinjector">
<br>
<img src="https://img.shields.io/badge/-Standard_2.1-5C2D91?logo=.net"/>
<img src="https://img.shields.io/badge/-8.0-239120?logo=c-sharp"/>
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

If you want to use an extremely easy dependency injection framework, use the latest features the C# language has to offer, or work with assemblies, then *StackInjector* is made for you!


## Installation

Visit the [Nuget page](https://www.nuget.org/packages/StackInjector)

## Usage

What follows is a brief introduction for versions above 4.0, in-depth tutorials and explanations can be found in the repository's [Wiki](https://github.com/JacopoWolf/StackInjector/wiki)

---

Plan your components as **interfaces**!

```cs
interface IFoo
{
    string SomeMethod( string element );
}
```

And then completely forget about constructors!

```cs
using StackInjector.Attributes;

[Service]
class SimpleFoo : IFoo
{
    // use the specific class
    [Served]
    FooOptions options;

    // or let the library find an implementation!
    [Served]
    ILogger Logger { get; set; }

    // serve an Enumerable of every implementation of IMyFilter!
    [Served]
    IEnumerable<IMyFilter> filters; 
    

    public string SomeMethod( string element ) 
    {
        // those are just random examples!
        foreach ( var f in this.filters )
            f.Pass( element );
        
        if (this.options.something)
            this.Logger.Log(element)
        
        return element;
    }
}
```

Everything **must** be explicit (`[Service]`,`[Served]` and `[Ignored]` attributes) allowing for **extremely readable code** and for **granular control** over the injection process.

--- 

All instances are held inside a **StackWrapper**, which exposes useful methods to manage your classes.

To initialize your application in a wrapper, you have multiple options, and every single one of them type-safe! 

**synchronous**

a simple call to a static method and you're done!

```cs   
Injector.From<IMyAppEntryPoint>().Entry.DoWork();
```

**asynchronous**

Thead safe, `IDisposable`, and centralized!

```cs
// the "using" keyword here allows for safe disposing of the resources
//  and terminating pending tasks with a CancellationToken
using var app = Injector.AsyncFrom<IMyAsyncAppEntryPoint>( (e,i,t) => e.AsyncWork(i,t) );

// can be called from anywhere
app.Submit( "someInput" );

// waiting for completed tasks is this simple
await foreach ( var result in app.Elaborated() )
    Console.WriteLine( result );
```


## Contributing

Any contribution is appreciated! Especially bug reports, which mean you've been using this library I've been hard working on!

If you want to contribute code first read [contributing](CONTRIBUTING.md) and [code of conduct](CODE_OF_CONDUCT.md) (I promise they're short)

*suggested editor: ![visualStudio](https://img.shields.io/badge/-Visual_Studio-5C2D91?logo=visual-studio)*



## License

Distributed under the `General Public Licence v3.0`.

See [LICENSE](LICENSE).


## Acknowledgements

- [Shields.io](https://shields.io/)
- [Readme-Template](https://github.com/othneildrew/Best-README-Template)

---
initial project and logo by [@JacopoWolf](https://github.com/JacopoWolf), 04/2020
