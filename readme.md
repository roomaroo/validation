# Validation examples

This code is designed to demonstrate different ideas about how to do validation.

As an introduction, read this [discussion of validation](./AboutValidation.md).

Each example is in a different branch of the git repo. The branches are:
- `main` - no validation
- `2-fluent-validation` - basic validation using the [FluentValidation](https://fluentvalidation.net/),
but with no way for the calculation to know its inputs have been validated.
- `3-compiler-enforced-validation` - a parallel set of input classes makes it impossible to pass invalid data to the calculation.
- `4-source-generators` - use .NET 5 source generators to create the validation classes 

# No validation
This example does not have any validation. The calculation throws exceptions if the input data is invalid.

