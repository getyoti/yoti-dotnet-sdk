# Yoti .NET SDK Development Instructions

This document provides comprehensive guidelines for developing and maintaining the Yoti .NET SDK. Follow these instructions when implementing new features, fixing bugs, or making any core changes to the SDK.

## Table of Contents

- [Development Environment](#development-environment)
- [Code Structure](#code-structure)
- [Implementation Guidelines](#implementation-guidelines)
- [Testing Requirements](#testing-requirements)
- [Documentation Standards](#documentation-standards)
- [Code Quality](#code-quality)
- [Examples and Demos](#examples-and-demos)
- [Build and Release](#build-and-release)
- [Common Patterns](#common-patterns)
- [Troubleshooting](#troubleshooting)

## Development Environment

### Prerequisites
- .NET SDK 6.0 or later
- Visual Studio 2022 or Visual Studio Code
- Git for version control

### Project Structure
```
yoti-dotnet-sdk/
├── src/
│   ├── Yoti.Auth/              # Main SDK library
│   └── Examples/               # Example applications
├── test/
│   ├── Yoti.Auth.Tests/        # Unit tests
│   └── Yoti.Auth.Tests.Common/ # Test utilities
├── docs/                       # Documentation
└── INSTRUCTIONS.md             # This file
```

### Build Commands
```bash
# Build the entire solution
dotnet build src/Yoti.Auth.sln

# Build specific project
dotnet build src/Yoti.Auth/Yoti.Auth.csproj

# Run tests
dotnet test test/Yoti.Auth.Tests/Yoti.Auth.Tests.csproj
```

## Code Structure

### Namespace Organization
- `Yoti.Auth` - Core authentication and client functionality
- `Yoti.Auth.DocScan` - Document scanning functionality
- `Yoti.Auth.DigitalIdentity` - Digital identity services
- `Yoti.Auth.Aml` - Anti-money laundering services
- `Yoti.Auth.Share` - Dynamic sharing functionality

### File Naming Conventions
- Use PascalCase for all file names
- Match file names with the primary class they contain
- Use descriptive names that clearly indicate purpose

## Implementation Guidelines

### 1. Adding New Configuration Properties

When adding new configuration properties (like `suppressed_screens`):

#### Step 1: Update Configuration Model
```csharp
// In the configuration class (e.g., SdkConfig.cs)
[JsonProperty(PropertyName = "property_name")]
public PropertyType PropertyName { get; }
```

#### Step 2: Update Constructor
```csharp
public ConfigClass(
    // existing parameters,
    PropertyType propertyName = null)  // Add as optional parameter
{
    // existing assignments
    PropertyName = propertyName;
}
```

#### Step 3: Update Builder Pattern
```csharp
// In the builder class (e.g., SdkConfigBuilder.cs)
private PropertyType _propertyName;

public BuilderClass WithPropertyName(PropertyType propertyName)
{
    _propertyName = propertyName;
    return this;
}

public ConfigClass Build()
{
    return new ConfigClass(
        // existing parameters,
        _propertyName);
}
```

#### Step 4: Handle Code Analysis Warnings
If you get constructor parameter warnings, add suppressions to `GlobalSuppressions.cs`:
```csharp
[assembly: SuppressMessage("Design", "CA1025:Replace repetitive arguments with params array", 
    Justification = "Constructor parameters need to be explicit for builder pattern compatibility", 
    Scope = "member", Target = "~M:ClassName.#ctor(...)")]
```

### 2. Builder Pattern Implementation

Follow the established builder pattern:
- Use fluent interface (return `this`)
- Provide clear, descriptive method names starting with `With`
- Include comprehensive XML documentation
- Support method chaining

Example:
```csharp
/// <summary>
/// Sets the property description
/// </summary>
/// <param name="propertyName">Description of the parameter</param>
/// <returns>The <see cref="BuilderClassName"/></returns>
public BuilderClassName WithPropertyName(PropertyType propertyName)
{
    _propertyName = propertyName;
    return this;
}
```

### 3. JSON Serialization

- Use `[JsonProperty(PropertyName = "snake_case_name")]` for all public properties
- Follow the API specification for property naming
- Ensure backward compatibility with existing JSON schemas

### 4. Error Handling

- Use appropriate exception types from `Yoti.Auth.Exceptions`
- Provide meaningful error messages
- Include sufficient context for debugging
- Follow existing patterns for HTTP error handling

## Testing Requirements

### Unit Testing Standards

#### Test Class Organization
```csharp
[TestClass]
public class ClassNameTests
{
    // Test methods here
}
```

#### Test Method Naming
Use descriptive names that clearly indicate:
- What is being tested
- What conditions/inputs are used
- What the expected outcome is

Examples:
- `ShouldBuildWithPropertyName()`
- `ShouldHandleNullPropertyName()`
- `PropertyNameShouldBeNullIfNotSet()`

#### Required Test Coverage

For each new feature, implement tests for:

1. **Happy Path Tests**
   ```csharp
   [TestMethod]
   public void ShouldBuildWithNewProperty()
   {
       var expectedValue = "test-value";
       
       var result = new Builder()
           .WithNewProperty(expectedValue)
           .Build();
           
       Assert.AreEqual(expectedValue, result.NewProperty);
   }
   ```

2. **Null/Empty Handling**
   ```csharp
   [TestMethod]
   public void ShouldHandleNullNewProperty()
   {
       var result = new Builder()
           .WithNewProperty(null)
           .Build();
           
       Assert.IsNull(result.NewProperty);
   }
   ```

3. **Default Behavior**
   ```csharp
   [TestMethod]
   public void NewPropertyShouldBeNullIfNotSet()
   {
       var result = new Builder().Build();
       
       Assert.IsNull(result.NewProperty);
   }
   ```

4. **Edge Cases**
   ```csharp
   [TestMethod]
   public void ShouldHandleEmptyNewProperty()
   {
       var emptyList = new List<string>();
       
       var result = new Builder()
           .WithNewProperty(emptyList)
           .Build();
           
       Assert.IsNotNull(result.NewProperty);
       Assert.AreEqual(0, result.NewProperty.Count);
   }
   ```

5. **Builder Pattern Behavior**
   ```csharp
   [TestMethod]
   public void ShouldRetainLatestValueWithMultipleCalls()
   {
       var firstValue = "first";
       var secondValue = "second";
       
       var result = new Builder()
           .WithNewProperty(firstValue)
           .WithNewProperty(secondValue)
           .Build();
           
       Assert.AreEqual(secondValue, result.NewProperty);
   }
   ```

#### Test Dependencies
- Use `System.Linq` when needed for collections
- Use MSTest framework (`Microsoft.VisualStudio.TestTools.UnitTesting`)
- Use `Moq` for mocking external dependencies
- Follow existing test patterns in the codebase

### Integration Testing
- Create realistic examples in the Examples folder
- Test actual API integration scenarios
- Validate JSON serialization/deserialization

## Documentation Standards

### XML Documentation
All public APIs must include comprehensive XML documentation:

```csharp
/// <summary>
/// Brief description of what the method/property does
/// </summary>
/// <param name="parameterName">Description of the parameter</param>
/// <returns>Description of what is returned</returns>
/// <remarks>
/// Additional details, usage notes, or important information
/// </remarks>
/// <example>
/// Example usage:
/// <code>
/// var result = new Builder()
///     .WithProperty("value")
///     .Build();
/// </code>
/// </example>
```

### Code Comments
- Use comments to explain complex business logic
- Avoid obvious comments that just restate the code
- Include references to API documentation where relevant

### README Files
- Update relevant README files when adding new features
- Include code examples for new functionality
- Document breaking changes and migration paths

## Code Quality

### Static Analysis
The project uses several code analysis tools:
- Built-in .NET analyzers
- SonarQube rules
- Custom analysis rules

### Code Style
- Follow existing code formatting in the project
- Use meaningful variable and method names
- Keep methods focused and reasonably sized
- Use appropriate access modifiers

### Error Suppression
When suppressing analysis warnings:
1. Add to `GlobalSuppressions.cs` with clear justification
2. Use specific scoping (method/type level)
3. Document why the suppression is necessary

Example:
```csharp
[assembly: SuppressMessage("Design", "CA1025:Replace repetitive arguments with params array", 
    Justification = "Constructor parameters need to be explicit for builder pattern compatibility", 
    Scope = "member", Target = "~M:Yoti.Auth.DocScan.Session.Create.SdkConfig.#ctor(...)")]
```

## Examples and Demos

### Creating Examples
When adding new features, create comprehensive examples:

1. **Controller Examples**
   - Create new controllers in `src/Examples/DocScan/DocScanExample/Controllers/`
   - Include multiple usage scenarios
   - Add comprehensive documentation and comments

2. **View Examples**
   - Create corresponding views that demonstrate the feature
   - Include interactive elements where appropriate
   - Show different configuration options

3. **Documentation Examples**
   - Create dedicated README files for complex features
   - Include code snippets and usage patterns
   - Provide troubleshooting information

### Example Structure
```csharp
/// <summary>
/// Example controller demonstrating [feature name]
/// This example shows how to [primary use case]
/// </summary>
public class FeatureController : Controller
{
    /// <summary>
    /// [Scenario name] - [brief description]
    /// This example demonstrates [specific scenario]
    /// </summary>
    public IActionResult ScenarioMethod()
    {
        // Clear, well-commented implementation
        // Show configuration options
        // Include error handling
    }
}
```

## Build and Release

### Version Management
- Follow semantic versioning (SemVer)
- Update version numbers in project files
- Document breaking changes in release notes

### Multi-Framework Support
The SDK targets multiple frameworks:
- .NET 6.0
- .NET Standard 2.1
- .NET Standard 1.6
- .NET Framework versions

Ensure compatibility across all target frameworks.

### Build Validation
Before committing:
1. Build all target frameworks successfully
2. Run all unit tests
3. Verify examples compile and run
4. Check for code analysis warnings

## Common Patterns

### 1. Service Client Pattern
```csharp
public class ServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly AsymmetricCipherKeyPair _keyPair;
    
    public async Task<ResultType> MethodAsync(parameters)
    {
        // Implementation
    }
}
```

### 2. Result Objects
```csharp
public class OperationResult
{
    public PropertyType Property { get; }
    
    public OperationResult(PropertyType property)
    {
        Property = property;
    }
}
```

### 3. Builder Pattern
```csharp
public class ConfigBuilder
{
    private PropertyType _property;
    
    public ConfigBuilder WithProperty(PropertyType property)
    {
        _property = property;
        return this;
    }
    
    public Config Build()
    {
        return new Config(_property);
    }
}
```

### 4. Exception Handling
```csharp
try
{
    // Operation
}
catch (HttpRequestException ex)
{
    throw new YotiServiceException("Specific error message", ex);
}
```

## Troubleshooting

### Common Issues

#### Build Errors
- **Constructor parameter count warnings**: Add appropriate suppressions to `GlobalSuppressions.cs`
- **Missing using statements**: Add required namespaces (commonly `System.Linq` for collection operations)
- **Target framework conflicts**: Ensure code is compatible with all target frameworks

#### Test Failures
- **Collection comparison issues**: Use `CollectionAssert` for collection comparisons
- **Mock setup problems**: Use `MockBehavior.Loose` for complex scenarios
- **Async test issues**: Ensure proper async/await usage in test methods

#### API Integration Issues
- **JSON serialization**: Verify `JsonProperty` attributes match API specification
- **HTTP client disposal**: Use proper `using` statements or dependency injection
- **Authentication**: Ensure proper key pair and client configuration

### Debugging Tips
1. Use detailed logging for HTTP requests/responses
2. Validate JSON serialization with unit tests
3. Test with actual API endpoints in development
4. Use debugger breakpoints for complex logic

### Getting Help
- Check existing issues and pull requests
- Review similar implementations in the codebase
- Consult Yoti API documentation
- Reach out to the team for architectural questions

## Checklist for New Features

Use this checklist when implementing new features:

### Implementation
- [ ] Configuration model updated with new property
- [ ] Constructor updated with optional parameter
- [ ] Builder pattern implemented with `With*` method
- [ ] JSON property mapping configured
- [ ] Code analysis warnings addressed

### Testing
- [ ] Happy path tests implemented
- [ ] Null/empty value handling tested
- [ ] Default behavior tested
- [ ] Edge cases covered
- [ ] Builder pattern behavior tested
- [ ] All tests passing

### Documentation
- [ ] XML documentation added to all public APIs
- [ ] Code examples created
- [ ] README files updated
- [ ] Usage scenarios documented

### Examples
- [ ] Controller examples created
- [ ] Multiple usage scenarios demonstrated
- [ ] Views and documentation created
- [ ] Examples tested and working

### Quality
- [ ] Code follows existing patterns
- [ ] Build succeeds on all target frameworks
- [ ] No new code analysis warnings
- [ ] Backward compatibility maintained

---

## Conclusion

Following these instructions will help maintain consistency, quality, and reliability across the Yoti .NET SDK. When in doubt, look at existing implementations for guidance and don't hesitate to ask questions.

Remember: **Quality over speed** - taking time to implement features correctly saves time in maintenance and support later.
