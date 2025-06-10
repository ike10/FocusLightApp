using System.Collections.ObjectModel;
using FocusLightApp.Models;

namespace FocusLightApp.Services;

/// <summary>
/// Interface for EEG simulation service
/// </summary>
public interface IEEGSimulationService
{
    event EventHandler<FocusData>? FocusDataReceived;
    bool IsRunning { get; }
    ObservableCollection<FocusData> History { get; }
    Task StartSimulationAsync();
    Task StopSimulationAsync();
    void InjectFocusSignal(bool isFocused);
    void ClearHistory();
}

/// <summary>
/// Simulates EEG focus detection signals from an EPOC headset
/// </summary>
public class EEGSimulationService : IEEGSimulationService, IDisposable
{
    private readonly Random _random = new();
    private Timer? _timer;
    private bool _isRunning;
    private readonly object _lockObject = new();

    /// <summary>
    /// Event fired when new focus data is received
    /// </summary>
    public event EventHandler<FocusData>? FocusDataReceived;

    /// <summary>
    /// Indicates if the simulation is currently running
    /// </summary>
    public bool IsRunning
    {
        get
        {
            lock (_lockObject)
            {
                return _isRunning;
            }
        }
        private set
        {
            lock (_lockObject)
            {
                _isRunning = value;
            }
        }
    }

    /// <summary>
    /// Collection of historical focus data readings
    /// </summary>
    public ObservableCollection<FocusData> History { get; } = new();

    /// <summary>
    /// Simulation interval in milliseconds (default: 2000ms)
    /// </summary>
    public int IntervalMs { get; set; } = 2000;

    /// <summary>
    /// Probability of generating a focused signal (default: 70%)
    /// </summary>
    public double FocusProbability { get; set; } = 0.7;

    /// <summary>
    /// Maximum number of history items to keep (default: 50)
    /// </summary>
    public int MaxHistoryItems { get; set; } = 50;

    /// <summary>
    /// Starts the EEG simulation
    /// </summary>
    public Task StartSimulationAsync()
    {
        return Task.Run(() =>
        {
            lock (_lockObject)
            {
                if (_isRunning) return;

                _timer = new Timer(GenerateSignal, null, 0, IntervalMs);
                IsRunning = true;
            }
        });
    }

    /// <summary>
    /// Stops the EEG simulation
    /// </summary>
    public Task StopSimulationAsync()
    {
        return Task.Run(() =>
        {
            lock (_lockObject)
            {
                if (!_isRunning) return;

                _timer?.Dispose();
                _timer = null;
                IsRunning = false;
            }
        });
    }

    /// <summary>
    /// Manually inject a focus signal for testing
    /// </summary>
    /// <param name="isFocused">True for focused state, false for unfocused</param>
    public void InjectFocusSignal(bool isFocused)
    {
        var signal = isFocused ? 1 : 0;
        var focusData = new FocusData(signal);
        ProcessFocusData(focusData);
    }

    /// <summary>
    /// Clears the signal history
    /// </summary>
    public void ClearHistory()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            History.Clear();
        });
    }

    /// <summary>
    /// Timer callback that generates random focus signals
    /// </summary>
    private void GenerateSignal(object? state)
    {
        try
        {
            // Generate signal based on probability (70% focused, 30% unfocused by default)
            var signal = _random.NextDouble() < FocusProbability ? 1 : 0;
            var focusData = new FocusData(signal);
            
            ProcessFocusData(focusData);
        }
        catch (Exception ex)
        {
            // Log error in production app
            System.Diagnostics.Debug.WriteLine($"Error generating signal: {ex.Message}");
        }
    }

    /// <summary>
    /// Processes new focus data and updates history
    /// </summary>
    private void ProcessFocusData(FocusData focusData)
    {
        // Update history on UI thread
        MainThread.BeginInvokeOnMainThread(() =>
        {
            try
            {
                // Add to beginning of collection for newest-first display
                History.Insert(0, focusData);

                // Maintain maximum history size
                while (History.Count > MaxHistoryItems)
                {
                    History.RemoveAt(History.Count - 1);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating history: {ex.Message}");
            }
        });

        // Fire event on background thread
        try
        {
            FocusDataReceived?.Invoke(this, focusData);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error firing event: {ex.Message}");
        }
    }

    /// <summary>
    /// Disposes of the service resources
    /// </summary>
    public void Dispose()
    {
        _timer?.Dispose();
        _timer = null;
        IsRunning = false;
        GC.SuppressFinalize(this);
    }
}