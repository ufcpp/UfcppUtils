using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Watcher = System.IO.FileSystemWatcher;

namespace Ufcpp.FileSystemWatcher
{
    /// <summary>
    /// read a file and deserialize its content on <see cref="System.IO.FileSystemWatcher.Changed"/>.
    /// </summary>
    /// <typeparam name="T">deserialize object type</typeparam>
    public class Loader<T> : IDisposable
    {
        private readonly string _filePath;
        private readonly Watcher _watcher;
        private readonly Func<Stream, Task<T>> _deserializer;
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">path to a file</param>
        /// <param name="deserializer">delegate to deserialize a content in the file</param>
        public Loader(string filePath, Func<Stream, Task<T>> deserializer)
        {
            _filePath = Path.GetFullPath(filePath);
            _watcher = new Watcher(Path.GetDirectoryName(_filePath));
            _deserializer = deserializer;

            _watcher.Changed += FileChanged;
            Load();
            _watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// deserialized object.
        /// </summary>
        public T Content { get; private set; }

        /// <summary>
        /// file changed and <see cref="Content"/> updated successfully.
        /// </summary>
        public event Action<T> Changed;

        public void Dispose()
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Changed -= FileChanged;
        }

        private void FileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath == _filePath)
                Load();
        }

        private async void Load()
        {
            try
            {
                await Task.Delay(100);

                await _lock.WaitAsync();

                if (!File.Exists(_filePath)) return;

                using (var s = File.Open(_filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    Content = await _deserializer(s);
                    Changed?.Invoke(Content);
                }
            }
            catch { }
            finally
            {
                _lock.Release();
            }
        }
    }
}
