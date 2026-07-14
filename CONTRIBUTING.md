# Contributing

Contributions are welcome through focused issues and pull requests.

## Development

1. Install the .NET 10 SDK.
2. Fork and clone the repository.
3. Create a branch from `master` (or `main`, if the default branch has been renamed).
4. Make the smallest change that solves the problem.
5. Add or update tests for behavioral changes.
6. Run the local quality checks:

```bash
dotnet format --verify-no-changes
dotnet build --configuration Release
dotnet test --configuration Release
```

## Pull requests

Describe the motivation, the approach, and how the change was verified. Keep
unrelated refactoring in a separate pull request so reviews remain clear.

By contributing, you agree that your contribution is licensed under the
repository's [MIT License](LICENSE).
