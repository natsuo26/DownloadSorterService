
using Microsoft.Extensions.Options;

namespace DownloadSorterService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IOptions<DownloadSorterOptions> _options;
        private FileSystemWatcher? _watcher;
        private string? _downloadsPath;
        private Dictionary<string, List<string>>? _fileCategories;

        public Worker(ILogger<Worker> logger, IOptions<DownloadSorterOptions> options)
        {
            _logger = logger;
            _options = options;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _downloadsPath = _options.Value.DownloadPath;
            _fileCategories = _options.Value.FileCategories;

            _watcher = new FileSystemWatcher(_downloadsPath);
            _watcher.Created += OnChanged;
            _watcher.Renamed += OnChanged;
            _watcher.EnableRaisingEvents = true;

            _logger.LogInformation("Watching folder: " + _downloadsPath);
            return base.StartAsync(cancellationToken);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Task.Delay(1000).ContinueWith(_ =>
            {
                try
                {
                    if (File.Exists(e.FullPath))
                    {
                        string extension = Path.GetExtension(e.Name!).ToLower();

                        if (_options.Value.ExcludedExtensions.Contains(extension))
                        {
                            _logger.LogInformation($"Skipped excluded file: {e.Name}");
                            return;
                        }

                        string targetFolder = _fileCategories!
                            .FirstOrDefault(kv => kv.Value.Contains(extension)).Key ?? "Misc";

                        string destDir = Path.Combine(_downloadsPath!, targetFolder);
                        Directory.CreateDirectory(destDir);
                        string destPath = Path.Combine(destDir, e.Name!);

                        if (File.Exists(destPath))
                        {
                            string newName = Path.GetFileNameWithoutExtension(e.Name) + "_" + Guid.NewGuid() + Path.GetExtension(e.Name);
                            destPath = Path.Combine(destDir, newName);
                        }

                        File.Move(e.FullPath, destPath);
                        _logger.LogInformation($"Moved {e.Name} to {targetFolder}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error moving file");
                }
            });
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _watcher?.Dispose();
            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
