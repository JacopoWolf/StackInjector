<h1> 
    <img src="logo/StackInjector_logo_notext.svg" height="35px" /> 
    StackInjector
</h1>
 

A **self-contained**, **performing**, **easy-to-use** dependency injection library, written for the **.NET Standard 2.1**

![badges here!](https://img.shields.io/badge/badges-here-inactive)

## Roadmap

roadmap to the next major version:

![GitHub milestone](https://img.shields.io/github/milestones/progress-percent/jacopowolf/stackinjector/2)
![GitHub milestone](https://img.shields.io/github/milestones/progress-percent/jacopowolf/stackinjector/3)
![GitHub milestone](https://img.shields.io/github/milestones/progress-percent/jacopowolf/stackinjector/1)

## Installation

![working on it](https://img.shields.io/badge/status-in_progress-orange)

## Usage

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
