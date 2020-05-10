<h1> 
    <img src="logo/StackInjector_logo_notext.svg" height="35px" /> 
    StackInjector
</h1>


![dotnetstandard2.1](https://img.shields.io/badge/-Standard_2.1-5C2D91?logo=.net) 
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
Or visit: https://www.nuget.org/packages/StackInjector

## Usage :wrench:

For in-depth tutorials and explanations visit the [wiki](https://github.com/JacopoWolf/StackInjector/wiki/Tutorial)

Below are shown the main features

---

Plan your components as **interfaces** and *implement* them! Forget constructors, forget access modifiers!

```cs
interface IMyService
{
    string Filter( string element );
}
```
```cs
[Service]
class MySimpleService : IMyService
{
    [Served]
    IMyDatabaseAccess Database { get; set; }
    
    [Served]
    IMyOtherSubFilter MinorFilter { get; set; }
    
    string Filter( string element ) 
    {
        this.Database.SomeMethod( element );
        this.MinorFilter.SomeOtherMethod( element );
        // do something here
        return element;
    }
}
```

Then to initialize, after implementing an entry point `[Service]`, it's as simple as:
```cs
using StackInjector;
```

```cs
Injector.From<IMyAppEntryPoint>().Start();
```

Since `From<T>()` requires `T` to implement `IStackEntryPoint` you have full control of the execution flow of your application, being also allowed to instantiate a new self-contained automatically wired up stack at every point in your app and wait for a result! 

```cs
var myString = Injector.From<ISomeOperationEntryPoint>().Start<string>();
```

For **more** information and in-depth **tutorials**, look at the simple tutoriars in the [wiki](https://github.com/JacopoWolf/StackInjector/wiki)


## Contributing :pencil2:

Read [contributing](CONTRIBUTING.md) and [code of conduct](CODE_OF_CONDUCT.md)

Any contribution is appreciated! Thanks!

suggested editor: ![visualstudio](https://img.shields.io/badge/-Visual_Studio-5C2D91?logo=visual-studio)


## License

![GitHub](https://img.shields.io/github/license/jacopowolf/stackinjector)

---
initial project and logo by [@JacopoWolf](https://github.com/JacopoWolf)
