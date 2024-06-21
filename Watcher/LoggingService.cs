using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.UI.Dispatching;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LoggingService
{
    private readonly DispatcherQueue _dispatcherQueue;

    public LoggingService()
    {
        _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        Logs = new ObservableCollection<string>();
    }

    public ObservableCollection<string> Logs { get; }

    public void Log(string message)
    {
        if (_dispatcherQueue.HasThreadAccess)
        {
            Logs.Add(message);
        }
        else
        {
            _dispatcherQueue.TryEnqueue(() => Logs.Add(message));
        }
    }
}
