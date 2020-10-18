# Contributing to StackInjector

- [Principles](#principles)
- [Branch naming rules](#branch-naming-rules)
- [Code style](#code-style)
- [Wiki](#wiki)
- [Tests](#tests)


---

Hello! If you're reading this it means you're interested in contributing to my repository! 

First of all, thanks! 


## Principles

- :memo: **Always open an issue**. There's plenty of templates to choose from! (and if not, open an issue discussing a new one!)
- :o: **Keep it simple**. That's the philosophy i tried to follow when designing this library and it's what i hope will make it great!
- :heavy_division_sign: **Divide et impera**. When it seems like too much, divide it into many small tasks!
- :white_check_mark: **Test driven**. We do test-driven development. So write a test, and develop around it! Then write some more!

## Branch naming rules

|  naming   | description                                                          | examples                 |
| :-------: | -------------------------------------------------------------------- | ------------------------ |
| `master`  | default branch for documentation and releases                        |                          |
| `rel/M.m` | holds pre-releases for minor versions                                | `rel/3.0`                |
| `dev/**`  | for development of a specific feature. Could be named after an issue | `dev/feature`, `dev/#69` |
| `fix/**`  | used for bug fixing. could be named after an issue                   | `fix/#70`                |
| `doc/**`  | used for documentation. Must be linear and merged with master.       | `doc/README`             |


## Code style

We use **FXCop** as style reference, meaning we follow [Microsoft's coding conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions).

- give explanatory and complete names
- fields start with lowercase `myFieldName`
- properties, classes and methods with uppercase `SomeCoolMethod()`
- interfaces MUST start with an `I`

Some project specifics:
- always create an interface for public APIs and hide it's behavior with an internal class


## Wiki

If you want to contribute to the Wiki, feel free to! Just don't push stupid updates. 

Grammar corrections are more than welcome.


## Tests

You can also help writing a new test! Find bugs! Try to break things!

Bit first:
- **BlackBox**: how the user would use the classes, without knowing the internal state
- **WhiteBox**: to test single methods and various possible internal states
- *External Assembly*: an external empty project to test external assembly imports