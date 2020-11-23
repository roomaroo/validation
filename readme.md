# Validation examples

This code is designed to demonstrate different ideas about how to do validation.

As an introduction, read this [discussion of validation](./AboutValidation.md).

Each example is in a different branch of the git repo. The branches are:
- `main` - no validation
- `2-fluent-validation` - basic validation using the [FluentValidation](https://fluentvalidation.net/),
but with no way for the calculation to know its inputs have been validated.
- `3-compiler-enforced-validation` - a parallel set of input classes makes it impossible to pass invalid data to the calculation.
- `4-source-generators` - use .NET 5 source generators to create the validation classes 

# Fluent Validation
This example has validation, and uses the [FluentValidation](https://fluentvalidation.net/) library. This makes it easier to write validation rule, and chain rules together.

The calculation has to run the validation itself, because it has no way of knowing whether its inputs have already been validated.



