using DownloadSorterService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.WindowsServices;

Host.CreateDefaultBuilder(args)
  .UseWindowsService(options =>
  {
      options.ServiceName = "DownloadSorterService";
  })
  .ConfigureServices((hostContext, services) =>
  {
      services.Configure<DownloadSorterOptions>(
        hostContext.Configuration.GetSection("DownloadSorter"));
      services.AddHostedService<Worker>();
  })
  .Build()
  .Run();
