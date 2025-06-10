# EEG Focus Light App

A professional .NET MAUI application that simulates EEG focus detection from an EPOC headset, controlling a virtual light bulb based on focus state.

## Features

### Core Functionality

* **Real-time Signal Processing** : Receives and processes binary focus signals (1=focused, 0=unfocused)
* **Virtual Light Bulb** : Animated bulb with glow effects that responds to focus state
* **Automatic Simulation** : Timer-based signal generation with configurable probability
* **Manual Controls** : Override buttons for testing different focus states
* **Statistics Dashboard** : Real-time tracking of focus performance metrics
* **Signal History** : Scrollable log of recent readings with timestamps

### Visual Design

* **Modern Dark Theme** : Contemporary UI with smooth animations
* **Responsive Layout** : Adaptive to different screen sizes and orientations
* **Visual Feedback** : Color-coded status indicators and glow effects
* **Intuitive Controls** : Clear, accessible button layout
* **Real-time Updates** : Live statistics and status information

### Technical Architecture

* **MVVM Pattern** : Clean separation of concerns with data binding
* **Dependency Injection** : Proper service registration and lifecycle management
* **Event-Driven Architecture** : Reactive programming with observable collections
* **Cross-Platform** : Supports Android, iOS, Windows, and macOS
* **Thread-Safe** : Proper UI thread handling for real-time updates

## Project Structure

```
FocusLightApp/
├── Models/
│   └── FocusData.cs              # Signal data model
├── ViewModels/
│   └── MainViewModel.cs          # Main UI logic with MVVM
├── Views/
│   ├── MainPage.xaml            # Primary user interface
│   └── MainPage.xaml.cs         # Code-behind
├── Services/
│   └── EEGSimulationService.cs  # Signal generation service
├── Converters/
│   └── ValueConverters.cs       # UI data binding converters
├── App.xaml                     # Application resources
├── AppShell.xaml               # Navigation shell
└── MauiProgram.cs              # Dependency injection setup
```

## Setup Instructions

### Prerequisites

* .NET 8.0 SDK or later
* Visual Studio 2022 (recommended) or Visual Studio Code
* Platform-specific development tools:
  * **Android** : Android SDK, Android Emulator
  * **iOS** : Xcode (macOS only)
  * **Windows** : Windows 10/11 SDK
  * **macOS** : Xcode command line tools

### Installation Steps

1. **Clone or Create Project**
   ```bash
   # If using git
   git clone <repository-url>
   cd FocusLightApp

   # Or create new project
   dotnet new maui -n FocusLightApp
   ```
2. **Restore Packages**
   ```bash
   dotnet restore
   ```
3. **Build Project**
   ```bash
   dotnet build
   ```
4. **Run Application**
   ```bash
   # For Android
   dotnet build -t:Run -f net8.0-android

   # For Windows
   dotnet build -t:Run -f net8.0-windows10.0.19041.0

   # For iOS (macOS only)
   dotnet build -t:Run -f net8.0-ios

   # For macOS
   dotnet build -t:Run -f net8.0-maccatalyst
   ```

### Required NuGet Packages

The project uses the following packages (automatically restored):

* `CommunityToolkit.Mvvm` (8.2.2) - MVVM helpers and commands
* `Microsoft.Extensions.Logging.Debug` (8.0.0) - Debug logging support

## Usage Guide

### Getting Started

1. **Launch the Application** : Start the app on your target platform
2. **Start Simulation** : Tap "Start Simulation" to begin receiving mock EEG signals
3. **Observe Light Bulb** : Watch the virtual bulb change color and glow based on focus state
4. **Monitor Statistics** : View real-time focus rate and reading counts
5. **Review History** : Scroll through recent signal readings with timestamps

### Controls

* **Start/Stop Simulation** : Toggle automatic signal generation
* **Force Focus** : Manually inject a focused signal for testing
* **Force Unfocus** : Manually inject an unfocused signal for testing
* **Clear History** : Reset all statistics and signal history

### Understanding the Display

* **Gold Bulb with Glow** : Focused state (Signal = 1)
* **Gray Bulb** : Unfocused state (Signal = 0)
* **Focus Rate** : Percentage of focused readings
* **Total Readings** : Count of all signals received
* **Last Signal** : Timestamp of most recent reading

## Configuration Options

### Simulation Parameters

The `EEGSimulationService` can be configured with:

* **Interval** : Time between signals (default: 2000ms)
* **Focus Probability** : Chance of focused signal (default: 70%)
* **Max History** : Number of readings to keep (default: 50)

### Customization Examples

```csharp
// In MauiProgram.cs or service registration
builder.Services.AddSingleton<IEEGSimulationService>(provider =>
{
    var service = new EEGSimulationService
    {
        IntervalMs = 1000,           // 1 second intervals
        FocusProbability = 0.5,      // 50% focus rate
        MaxHistoryItems = 100        // Keep 100 readings
    };
    return service;
});
```

## Troubleshooting

### Common Issues

**App Won't Start**

* Ensure .NET 8.0 SDK is installed
* Check that all NuGet packages are restored
* Verify platform-specific tools are installed

**Simulation Not Working**

* Check that service is properly registered in DI container
* Verify timer disposal isn't preventing restart
* Ensure UI thread updates are working correctly

**UI Not Updating**

* Confirm data binding is properly configured
* Check that ObservableObject properties are implemented correctly
* Verify converters are registered in XAML resources

**Performance Issues**

* Monitor memory usage during long-running sessions
* Consider reducing history size if memory becomes an issue
* Check for proper disposal of timers and event handlers

### Debug Tips

* Enable debug logging to see service events
* Use breakpoints in `OnFocusDataReceived` to trace data flow
* Monitor UI thread usage with platform-specific profilers

## Extension Points

### Real Hardware Integration

The app is designed for easy extension to real EEG hardware:

```csharp
// Replace simulation service with real EEG service
public class EpocEEGService : IEEGSimulationService
{
    // Implement Bluetooth connection to EPOC headset
    // Parse real EEG signals
    // Convert to focus/unfocus binary signals
}
```

### Enhanced Features

* **Data Persistence** : Add SQLite for storing session data
* **Export Functionality** : Generate CSV reports of focus sessions
* **Custom Themes** : Implement user-selectable color schemes
* **Multiple Devices** : Support for multiple concurrent headsets
* **Cloud Sync** : Upload session data to cloud storage

### Advanced Analytics

* **Focus Patterns** : Analyze focus trends over time
* **Session Comparison** : Compare performance across sessions
* **Machine Learning** : Implement focus prediction algorithms
* **Biometric Integration** : Combine with heart rate or other sensors

## Development Notes

### Code Quality Standards

* All public APIs include XML documentation
* SOLID principles are followed throughout
* Error handling is comprehensive and graceful
* Memory management follows .NET best practices

### Performance Considerations

* UI updates are batched and optimized
* Timer resources are properly managed
* Observable collections use efficient update patterns
* Memory leaks are prevented through proper disposal

### Testing Recommendations

* Unit test the EEG simulation service
* Integration test the MVVM data binding
* UI test the cross-platform layouts
* Performance test with extended simulation runs

## License

This project is provided as a demonstration of .NET MAUI development practices and EEG interface design patterns. Modify and extend as needed for your specific requirements.

## Contributing

When extending this application:

1. Follow the established MVVM architecture
2. Maintain clean separation of concerns
3. Add comprehensive error handling
4. Include XML documentation for public APIs
5. Test across all target platforms

## Support

For technical questions about .NET MAUI development or EEG integration patterns, refer to:

* [.NET MAUI Documentation](https://docs.microsoft.com/en-us/dotnet/maui/)
* [MVVM Community Toolkit](https://docs.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)
* [Xamarin Community Forums](https://forums.xamarin.com/)

This application serves as a professional foundation for EEG-controlled interfaces and can be extended for research, therapy, or consumer applications.
