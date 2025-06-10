namespace FocusLightApp.Models;

/// <summary>
/// Represents a single EEG focus detection reading
/// </summary>
public class FocusData
{
    /// <summary>
    /// The raw signal value (0 = not focused, 1 = focused)
    /// </summary>
    public int Signal { get; set; }

    /// <summary>
    /// Timestamp when the signal was recorded
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Boolean representation of focus state
    /// </summary>
    public bool IsFocused => Signal == 1;

    /// <summary>
    /// Formatted timestamp for display
    /// </summary>
    public string FormattedTime => Timestamp.ToString("HH:mm:ss");

    /// <summary>
    /// Human-readable status text
    /// </summary>
    public string StatusText => IsFocused ? "Focused" : "Unfocused";

    /// <summary>
    /// Status color for UI display
    /// </summary>
    public Color StatusColor => IsFocused ? Colors.Green : Colors.Gray;

    /// <summary>
    /// Creates a new FocusData instance
    /// </summary>
    /// <param name="signal">The signal value (0 or 1)</param>
    /// <param name="timestamp">Optional timestamp (defaults to now)</param>
    public FocusData(int signal, DateTime? timestamp = null)
    {
        Signal = signal;
        Timestamp = timestamp ?? DateTime.Now;
    }

    /// <summary>
    /// Creates a focused signal instance
    /// </summary>
    /// <returns>FocusData with signal = 1</returns>
    public static FocusData CreateFocused() => new(1);

    /// <summary>
    /// Creates an unfocused signal instance
    /// </summary>
    /// <returns>FocusData with signal = 0</returns>
    public static FocusData CreateUnfocused() => new(0);

    public override string ToString()
    {
        return $"{FormattedTime}: {StatusText} (Signal: {Signal})";
    }
}