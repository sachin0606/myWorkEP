using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace EPGServiceBLL.Configuration
{
	public sealed class EPGWindowsFolderWatcher : IDisposable
	{
		public class WatcherFile
		{
			public string Name { get; private set; }

			public string Path { get; private set; }

			public DateTime UpdateTime { get; private set; }

			public WatcherFile(string name, string path)
			{
				Name = name;
				Path = path;
				UpdateTime = DateTime.Now;
			}
		}

		private Dictionary<string, WatcherFile> pool = new Dictionary<string, WatcherFile>();

		private FileSystemWatcher watcher = null;

		public string FolderPath { get; private set; }

		public bool Running { get; private set; }

		public void Start(string folderpath)
		{
			if (!Running)
			{
				watcher = new FileSystemWatcher(folderpath, "*.*");
				watcher.InternalBufferSize = 81920;
				watcher.IncludeSubdirectories = false;
				Running = true;
				watcher.Changed += watcher_Changed;
				watcher.Created += watcher_Changed;
				watcher.Renamed += watcher_Changed;
				watcher.Deleted += watcher_Deleted;
				watcher.EnableRaisingEvents = true;
			}
		}

		private void watcher_Changed(object sender, FileSystemEventArgs e)
		{
			if (e.Name.Contains('.'))
			{
				Set(new WatcherFile(e.Name, e.FullPath));
			}
		}

		private void watcher_Deleted(object sender, FileSystemEventArgs e)
		{
			Remove(new WatcherFile(e.Name, e.FullPath));
		}

		public void Remove(WatcherFile item)
		{
			if (Running && pool.ContainsKey(item.Name))
			{
				Monitor.Enter(pool);
				if (pool.ContainsKey(item.Name))
				{
					pool.Remove(item.Name);
				}
				Monitor.PulseAll(pool);
				Monitor.Exit(pool);
			}
		}

		public void Set(WatcherFile item)
		{
			if (Running)
			{
				Monitor.Enter(pool);
				pool[item.Name] = item;
				Monitor.PulseAll(pool);
				Monitor.Exit(pool);
			}
		}

		public WatcherFile Get()
		{
			WatcherFile result = null;
			try
			{
				Monitor.Enter(pool);
				while (true)
				{
					bool flag = true;
					if (!Running)
					{
						return null;
					}
					if (pool.Count == 0)
					{
						Monitor.Wait(pool);
						continue;
					}
					break;
				}
				KeyValuePair<string, WatcherFile> keyValuePair = pool.FirstOrDefault();
				result = keyValuePair.Value;
				pool.Remove(keyValuePair.Key);
			}
			catch (Exception)
			{
				result = null;
			}
			finally
			{
				Monitor.Exit(pool);
			}
			return result;
		}

		public int Count()
		{
			return pool.Count;
		}

		public void Dispose()
		{
			if (!Running)
			{
				return;
			}
			watcher.Dispose();
			lock (pool)
			{
				pool.Clear();
				Monitor.PulseAll(pool);
			}
		}
	}
}
