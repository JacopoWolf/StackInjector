<h1> 
    <img src="logo/StackInjector_logo_notext.svg" height="35px" /> 
    StackInjector
</h1>
 

A **simple**, **performing**, **easy-to-use** dependency injection library for you stack-structured applications, like custom servers, interpreters, etc.

![csharp](https://img.shields.io/badge/-Standard_2.1-purple?logo=.net) 

## Roadmap

| date | version |
|-:|-|
| \*08/05/2020 | ![GitHub milestone](https://img.shields.io/github/milestones/progress-percent/jacopowolf/stackinjector/2) |
| \*22/05/2020 | ![GitHub milestone](https://img.shields.io/github/milestones/progress-percent/jacopowolf/stackinjector/3) |
| 26/06/2020 | ![GitHub milestone](https://img.shields.io/github/milestones/progress-percent/jacopowolf/stackinjector/1) |

\* release date may vary for minor releases

## Installation

:construction: soon available!

## Usage

For in-depth tutorials and explanations visit the [wiki](https://github.com/JacopoWolf/StackInjector/wiki)

Below are shown the main features

---

Plan your components as **interfaces** and *implement* them, then it's as simple as:
```cs
using StackInjector;
```

```cs
Injector.From<IMyAppEntryPoint>().Start();
```

Since `From<T>()` requires `T` to extend/implement `IStackEntryPoint` you have full control of the execution flow of your application, being also allowed to instantiate a new self-contained automatically wired up stack at every point in your app and wait for a result! 

```cs
var myString = Injector.From<ISomeOperationEntryPoint>().Start<string>();
```

> in milestone 0.2.0 I'm planning to add `AsyncStart<string>()` and implementation versioning


## Contributing

Read [contributing](CONTRIBUTING.md) and [code of conduct](CODE_OF_CONDUCT.md)

Any contribution is appreciated! Thanks!

## License

![GitHub](https://img.shields.io/github/license/jacopowolf/stackinjector)

---
initial project and logo by [@JacopoWolf](https://github.com/JacopoWolf)
