# Validation examples

This code is designed to demonstrate different ideas about how to do validation.

As an introduction, read this [discussion of validation](./AboutValidation.md).

Each example is in a different branch of the git repo. The branches are:
- `main` - no validation
- `2-fluent-validation` - basic validation using the [FluentValidation](https://fluentvalidation.net/),
but with no way for the calculation to know its inputs have been validated.
- `3-compiler-enforced-validation` - a parallel set of input classes makes it impossible to pass invalid data to the calculation.
- `4-source-generators` - use .NET 5 source generators to create the validation classes 

# Compiler-enforced validation - with Source Generator

This is similar to the compiler-enforced validation example, where there is
a separate set of `Validated` classes.

However, in this example the `Validated` classes are generated from the DTOs by a .NET 5 Source Generator.

Source Generators are a new feature. They are run as part of the compilation process, and can generate new source code that is added to the program being compiled.

By generating the Validation classes, we get all the advantages of the compiler-enforced validation, but without the maintenance overheads.

The disadvantage is that you need to be using the .NET 5 tooling to build your solution.

