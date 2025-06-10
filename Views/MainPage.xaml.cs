using FocusLightApp.ViewModels;

namespace FocusLightApp.Views;

/// <summary>
/// Main page for the EEG Focus Detection App
/// </summary>
public partial class MainPage : ContentPage
{
    /// <summary>
    /// Initializes the main page
    /// </summary>
    /// <param name="viewModel">Main view model instance</param>
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    /// <summary>
    /// Handle page appearing - useful for starting background services
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        // Could be used to automatically start simulation when page appears
        // if (BindingContext is MainViewModel viewModel && !viewModel.IsSimulationRunning)
        // {
        //     viewModel.ToggleSimulationCommand.Execute(null);
        // }
    }

    /// <summary>
    /// Handle page disappearing - useful for cleanup
    /// </summary>
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        
        // Cleanup could be performed here if needed
        // For example, stopping simulation when navigating away
    }
}