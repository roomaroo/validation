# Validation examples

This code is designed to demonstrate different ideas about how to do validation.

As an introduction, read this [discussion of validation](./AboutValidation.md).

Each example is in a different branch of the git repo. The branches are:
- `main` - no validation
- `2-fluent-validation` - basic validation using the [FluentValidation](https://fluentvalidation.net/),
but with no way for the calculation to know its inputs have been validated.
- `3-compiler-enforced-validation` - a parallel set of input classes makes it impossible to pass invalid data to the calculation.
- `4-source-generators` - use .NET 5 source generators to create the validation classes 

# Compiler enforced validation
In this example, there is a parallel set of "validated" classes, with a one-to-one mapping to the DTOs

| DTO | Validated|
|:-----|:-----:|-----:|
|`DTO.WindFarm`|`Validated.WindFarm`|
|`DTO.Location`|`Validated.Location`|
|`DTO.Turbine`|`Validated.Turbine`|

The calculation takes a `Validated.WindFarm` as input.

The `Validated` classes are immutable, with `internal` constructors. Instances of the `Validated` classes can only be created by validation code, and once created, they cannot be changed.

This means that the inputs to the calculation are guaranteed (by the compiler) to have been through validation.

I used AutoMapper to map between the `DTO`s and `Validated` classes - this reduces the amount of repetitive code that needs to be written.

The advantage of this approach is that the calculation can be confident that it has valid inputs. The disadvantage is the need to write a maintain the extra `Validated` classes.