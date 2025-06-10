using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FocusLightApp.Models;
using FocusLightApp.Services;

namespace FocusLightApp.ViewModels;

/// <summary>
/// Main view model for the EEG Focus Detection App
/// </summary>
public partial class MainViewModel : ObservableObject
{
    private readonly IEEGSimulationService _eegService;

    #region Observable Properties

    [ObservableProperty]
    private bool _isFocused;

    [ObservableProperty]
    private bool _isSimulationRunning;

    [ObservableProperty]
    private string _connectionStatus = "Disconnected";

    [ObservableProperty]
    private int _totalReadings;

    [ObservableProperty]
    private int _focusedCount;

    [ObservableProperty]
    private double _focusPercentage;

    [ObservableProperty]
    private DateTime _lastSignalTime;

    [ObservableProperty]
    private string _lastSignalTimeText = "Never";

    #endregion

    #region Commands

    [RelayCommand]
    private async Task ToggleSimulation()
    {
        try
        {
            if (IsSimulationRunning)
            {
                await _eegService.StopSimulationAsync();
                ConnectionStatus = "Disconnected";
            }
            else
            {
                await _eegService.StartSimulationAsync();
                ConnectionStatus = "Connected - Receiving Signals";
            }
            
            IsSimulationRunning = _eegService.IsRunning;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error toggling simulation: {ex.Message}");
            ConnectionStatus = "Error";
        }
    }

    [RelayCommand]
    private void InjectFocusedSignal()
    {
        _eegService.InjectFocusSignal(true);
    }

    [RelayCommand]
    private void InjectUnfocusedSignal()
    {
        _eegService.InjectFocusSignal(false);
    }

    [RelayCommand]
    private void ClearHistory()
    {
        _eegService.ClearHistory();
        ResetStatistics();
    }

    [RelayCommand]
    private void ResetStatistics()
    {
        TotalReadings = 0;
        FocusedCount = 0;
        FocusPercentage = 0.0;
        LastSignalTimeText = "Never";
    }

    #endregion

    /// <summary>
    /// History of signal readings from the service
    /// </summary>
    public ObservableCollection<FocusData> SignalHistory => _eegService.History;

    /// <summary>
    /// Initializes the main view model
    /// </summary>
    /// <param name="eegService">EEG simulation service</param>
    public MainViewModel(IEEGSimulationService eegService)
    {
        _eegService = eegService ?? throw new ArgumentNullException(nameof(eegService));
        
        // Subscribe to focus data events
        _eegService.FocusDataReceived += OnFocusDataReceived;
        
        // Initialize state
        IsSimulationRunning = _eegService.IsRunning;
        UpdateConnectionStatus();
    }

    /// <summary>
    /// Handles new focus data from the EEG services and updates the UI accordingly
    /// This method is called on the main thread to ensure UI updates are safe.
    /// </summary>
    private void OnFocusDataReceived(object? sender, FocusData focusData)
    {
        // Update UI on main thread
        MainThread.BeginInvokeOnMainThread(() =>
        {
            try
            {
                // Update current focus state
                IsFocused = focusData.IsFocused;
                
                // Update statistics
                TotalReadings++;
                if (focusData.IsFocused)
                {
                    FocusedCount++;
                }
                
                // Calculate percentage
                FocusPercentage = TotalReadings > 0 ? (double)FocusedCount / TotalReadings * 100 : 0;
                
                // Update last signal time
                LastSignalTime = focusData.Timestamp;
                LastSignalTimeText = focusData.FormattedTime;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating UI: {ex.Message}");
            }
        });
    }

    /// <summary>
    /// Updates the connection status display
    /// </summary>
    private void UpdateConnectionStatus()
    {
        ConnectionStatus = IsSimulationRunning ? "Connected - Receiving Signals" : "Disconnected";
    }

    /// <summary>
    /// Cleanup when view model is disposed
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_eegService != null)
            {
                _eegService.FocusDataReceived -= OnFocusDataReceived;
            }
        }
    }

    /// <summary>
    /// Public dispose method
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}