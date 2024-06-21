using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FolderMonitor
{
    private FileSystemWatcher _watcher;
    private Action<string> _onNewFile;

    public FolderMonitor(string folderPath,  Action<string> onNewFile)
    {
        _onNewFile = onNewFile;
        _watcher = new FileSystemWatcher(folderPath)
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
        };

        _watcher.Created += OnCreated;
        _watcher.EnableRaisingEvents = true;
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        _onNewFile.Invoke(e.FullPath);
    }
}