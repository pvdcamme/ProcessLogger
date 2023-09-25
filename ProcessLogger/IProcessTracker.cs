namespace ProcessLogger
{
    // Tracks cpu-usage of a single Process.
    public interface IProcessTracker
    {
        string Name { get; }

        float GetProcessorTime();
        bool IsFailing();
    }
}