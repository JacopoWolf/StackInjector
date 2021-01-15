# Contributing to StackInjector

- [Principles](#principles)
- [Branch naming rules](#branch-naming-rules)
- [Code style](#code-style)
- [Tests](#tests)


---

Hello! If you're reading this it means you're interested in contributing to my repository! 

First of all, thanks! 


## Principles

- :memo: **Always open an issue**; or a **discussion** for a more open ended confrontation
- :o: **Keep it simple**. That's the philosophy i tried to follow when designing this library and it's what i hope will make it great to use!
- :heavy_division_sign: **Divide et impera**. When it seems like too much, divide it into many small tasks!
- :white_check_mark: **Test driven**. We do test-driven development. So write a test, and develop around it! Then write some more!

## Branch naming rules

|  naming  | description                                                          | examples                 |
| :------: | -------------------------------------------------------------------- | ------------------------ |
| `master` | default branch for documentation and major releases                  |                          |
| `rel/M`  | holds pre and minor releases                                      | `rel/2`, `rel/3`                  |
| `dev/**` | for development of a specific feature. Could be named after an issue | `dev/#69`, `dev/cleanup`, `dev/feature-name` |
| `fix/**` | used for bug fixing. Should be named after an issue                   | `fix/#70`                |
| `doc/**` | used for documentation. Must be linear and merged with master.       | `doc/README`, `doc/reorg`             |


## Code style

We follow [Microsoft's coding conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions).

- give explanatory and complete names
- fields start with lowercase `myFieldName`
- properties, classes and methods with uppercase `SomeCoolMethod()`
- interfaces MUST start with an `I`
- use tabs for indentation, spaces for alignment
- APIs implementations must be internal


## Tests

You can also help writing a new test! Find bugs! Try to break things!

Tests structure:
- **BlackBox**: how the user would use the classes, without knowing the internal state
  - Features: tests for secondary features of StackWrappers
  - Use Cases: tests for possible irl use cases
- *External Assembly*: an external empty project to test external assembly imports