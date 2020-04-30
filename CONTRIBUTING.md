<h1>
    Contributing to
    <img src="logo/StackInjector_logo_notext.svg" height="32px" /> 
    StackInjector
</h1>


- [Introduction :green_book:](#introduction-green_book)
- [Naming rules :bookmark:](#naming-rules-bookmark)
  - [branches](#branches)
  - [classes](#classes)
- [Wiki :book:](#wiki-book)
- [Tests and Examples :microscope:](#tests-and-examples-microscope)


---

## Introduction :green_book:

Hello! If you're reading this it means you're interested in contributing to my repository! Thanks! 

Now, before you read the brief rules, a couple of tips:

- when in doubt, **open an issue**. There's plenty of templates to choose from! (and if not, open an issue discussing a new one!)
- **Keep it simple**! That's the philosophy i tried to follow when designing this library and it's what i hope will make it great!
- *Divide et impera*! When it seems like too much, divide it into many small tasks!

## Naming rules :bookmark:

### branches

- `master`: default branch for major releases
- *development*
  - `dev/M.m`: branch for holding changes of a specific version before merging to master. Code *documentation* can be added here.
  - `dev/feature` or `dev/M.m/feature`: for development of a specific feature. If it is specific to a target future version (see milestones)
- *bugs*
  - `fix/12345` where *12345* is replaced by the relative issue id

### classes

We use FXCop as style reference, meaning we follow [Microsoft's coding conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions).

Added to that, a couple of notes:
- *Divide et impera*, separate classes in multiple files when possible, naming them `TheClassName.whatshere.cs`, except for the main file that goes only with the class name only (`TheClassName.cs`).

## Wiki :book:

If you want to contribute to the Wiki, feel free to! Just don't push stupid updates. 

Grammar corrections are more than welcome

## Tests and Examples :microscope:

You can also help writing a new test! Find bugs! Try to beak things!

The great thing with this type of library is that every test project can be considered like an example project (except for the more advanced specific ones. Duh...)