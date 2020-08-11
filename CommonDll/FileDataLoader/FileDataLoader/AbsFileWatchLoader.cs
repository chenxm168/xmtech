using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDataLoader
{
  public abstract class AbsFileWatchLoader
    {
      public ICustomizedLog Clog
      { get; set; }

      public string FilePath
      {
          get;
          set;
      }

      public string FileFilter
      { get; set; }


      public void WatcherStrat()
      {

          FileSystemWatcher watcher = new FileSystemWatcher();
          watcher.Path = FilePath;
          watcher.Filter = FileFilter;
          watcher.Changed += new FileSystemEventHandler(OnChanged);
          watcher.Created += new FileSystemEventHandler(OnCreated);
          watcher.Deleted += new FileSystemEventHandler(OnDeleted);
          watcher.Renamed += new RenamedEventHandler(OnRenamed);
          watcher.EnableRaisingEvents = true;
          watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
          watcher.IncludeSubdirectories = true;
      }



      protected virtual void OnProcess(object source, FileSystemEventArgs e)
      {
          if (e.ChangeType == WatcherChangeTypes.Created)
          {
              OnCreated(source, e);


          }
          else if (e.ChangeType == WatcherChangeTypes.Changed)
          {
              OnChanged(source, e);

          }
          else if (e.ChangeType == WatcherChangeTypes.Deleted)
          {
              OnDeleted(source, e);
          }
      }
      protected abstract void OnRenamed(object source, RenamedEventArgs e);
      protected abstract void OnCreated(object source, FileSystemEventArgs e);
      protected abstract void OnChanged(object source, FileSystemEventArgs e);

      protected abstract void OnDeleted(object source, FileSystemEventArgs e);

    }
}
