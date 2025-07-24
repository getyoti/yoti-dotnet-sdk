# Suppressed Screens Configuration

This example demonstrates how to use the new `suppressed_screens` configuration property to customize the ID verification flow in Yoti's DocScan SDK.

## Overview

The `suppressed_screens` feature allows you to skip specific screens in the document scanning flow, creating a shortened or customized user experience. This is particularly useful for:

- **Returning users** who are familiar with the process
- **Mobile applications** where screen real estate is limited  
- **Streamlined workflows** for specific use cases
- **Custom integrations** requiring tailored user experiences

## Features

### 1. Shortened Flow Example
- Suppresses common introductory screens
- Optimized for returning users
- Maintains security while reducing friction

### 2. Custom Flow Example  
- Demonstrates advanced screen suppression
- Shows custom branding options
- Highly tailored user experience

### 3. Standard Flow Example
- No screens suppressed for comparison
- Full instructional flow
- Complete user guidance

## Configuration

### Basic Usage

```csharp
// Define which screens to suppress
var suppressedScreens = new List<string>
{
    "welcome",
    "privacy_policy", 
    "document_selection",
    "instructions",
    "tutorial"
};

// Configure SDK with suppressed screens
var sdkConfig = new SdkConfigBuilder()
    .WithAllowsCameraAndUpload()
    .WithPrimaryColour("#2d9fff")
    .WithSuppressedScreens(suppressedScreens)  // NEW: Add suppressed screens
    .Build();
```

### Integration with Session Specification

```csharp
var sessionSpec = new SessionSpecificationBuilder()
    .WithClientSessionTokenTtl(600)
    .WithResourcesTtl(96400)
    .WithUserTrackingId("user-tracking-id")
    
    // Add required checks
    .WithRequestedCheck(
        new RequestedDocumentAuthenticityCheckBuilder()
        .WithManualCheckFallback()
        .Build()
    )
    
    // Add required tasks
    .WithRequestedTask(
        new RequestedTextExtractionTaskBuilder()
        .WithManualCheckFallback()
        .Build()
    )
    
    // Configure SDK with suppressed screens
    .WithSdkConfig(sdkConfig)
    .Build();

// Create session
CreateSessionResult result = docScanClient.CreateSession(sessionSpec);
```

## Supported Screen Names

The following screen names are commonly supported for suppression:

| Screen Name | Description |
|-------------|-------------|
| `welcome` | Initial welcome/introduction screen |
| `privacy_policy` | Privacy policy acceptance screen |
| `document_selection` | Document type selection screen |
| `instructions` | Detailed instruction screens |
| `tutorial` | Tutorial/guidance screens |
| `preview_screen` | Document preview confirmation |
| `confirmation_screen` | Final confirmation screens |

> **Note**: The exact screen names may vary depending on your Yoti configuration and the specific flow being used. Consult with your Yoti integration team to determine the appropriate screen names for your use case.

## Running the Examples

1. **Prerequisites**: 
   - Ensure you have valid Yoti DocScan credentials configured
   - Update the `.env` file with your API credentials

2. **Run the application**:
   ```bash
   cd DocScanExample
   dotnet run
   ```

3. **Navigate to examples**:
   - Visit `/SuppressedScreens/Index` to see all examples
   - Try different flows to compare user experiences

## Implementation Notes

### Validation
- The `suppressed_screens` property accepts an array of strings
- Invalid screen names are ignored silently
- Empty arrays are valid and result in standard flow

### Flow Logic
- The flow controller checks for the presence of `suppressed_screens`
- Screens listed in the array are skipped during the flow
- Essential security screens cannot be suppressed

### Testing
- Each example includes different screen suppression configurations
- Compare flows to understand the impact of different configurations
- Test with different user scenarios (new vs returning users)

## Best Practices

1. **Start Conservative**: Begin with suppressing only non-essential screens like welcome and tutorial
2. **User Segmentation**: Use different configurations for new vs returning users
3. **A/B Testing**: Test different configurations to optimize conversion rates
4. **Security First**: Never suppress screens that are required for security compliance
5. **Documentation**: Keep track of which screens are suppressed for different user journeys

## API Reference

### SdkConfigBuilder.WithSuppressedScreens()

```csharp
public SdkConfigBuilder WithSuppressedScreens(IList<string> suppressedScreens)
```

**Parameters:**
- `suppressedScreens`: List of screen names to suppress during the flow

**Returns:**
- The `SdkConfigBuilder` instance for method chaining

**Example:**
```csharp
var builder = new SdkConfigBuilder()
    .WithSuppressedScreens(new List<string> { "welcome", "tutorial" });
```

## Troubleshooting

### Common Issues

1. **Screens not being suppressed**: 
   - Verify screen names are correct
   - Check with Yoti support for valid screen names

2. **Flow behaving unexpectedly**:
   - Ensure essential screens are not suppressed
   - Test with standard flow first

3. **Integration issues**:
   - Verify SDK version supports suppressed_screens
   - Check API credentials and permissions

## Support

For questions about specific screen names or advanced configuration options, please contact the Yoti integration team or refer to the official Yoti DocScan documentation.
