using System.IO;
using System.Windows;
using CineQuebec.Application;
using CineQuebec.Persistence;
using CineQuebec.Windows.View;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CineQuebec.Windows;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    internal static IServiceProvider ServiceProvider { get; set; } = default!;

    protected override void OnStartup (StartupEventArgs e)
    {
        var configuration = GetConfiguration();
        var serviceCollection = ConfigureServices(configuration);

        ServiceProvider = serviceCollection.BuildServiceProvider();

        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
            .Build();
    }

    private static IServiceCollection ConfigureServices (IConfiguration configuration)
    {
        return new ServiceCollection()
            .AddPersistenceServices(configuration)
            .AddApplicationServices()
            .AddTransient(typeof(MainWindow));
    }
}