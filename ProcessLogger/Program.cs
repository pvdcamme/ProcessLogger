// A very early prototype to gather log the process run-times


using ProcessLogger;

var log = FileLog.FileLog.InUserDocuments("logged_process.txt");

SystemTracker tracker = new((string name) => { return new ProcessTracker(name); });

while (true)
{
    tracker.MergeUnknownProcesses(SystemTracker.ProcessNames());

    foreach ((string name, float load) in tracker.GetTrackedProcesses())
    {
        log.AddEntry(name, load);

    }
    Thread.Sleep(10000);
}
