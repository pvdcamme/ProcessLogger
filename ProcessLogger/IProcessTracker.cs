namespace ProcessLogger
{
    public interface IProcessTracker
    {
        string Name { get; }

        float GetProcessorTime();
        bool IsFailing();
    }
}