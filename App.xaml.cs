using FocusLightApp.Views;

namespace FocusLightApp;

/// <summary>
/// Main application class
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Initializes the application
    /// </summary>
    public App()
    {
        InitializeComponent();
        
        // Set the main page
        MainPage = new AppShell();
    }

    /// <summary>
    /// Handle application lifecycle events
    /// </summary>
    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);
        
        // Configure window properties
        window.Title = "EEG Focus Light";
        
        return window;
    }
}