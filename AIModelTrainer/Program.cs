using AIModelTrainer.Application;
using AIModelTrainer.Services;
using AIModelTrainer.Handler;
using Microsoft.Extensions.DependencyInjection;
using AIModelTrainer.Navigation;

namespace AIModelTrainer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var app = serviceProvider.GetRequiredService<ModelTrainerApp>();
            app.StartTrainingAsync().GetAwaiter().GetResult();           
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ConfigurationService>();
            services.AddSingleton<FileService>();
            services.AddSingleton<ConsoleMenu>();
            services.AddSingleton<ImageInputHandler>();            
            services.AddSingleton<ModelTrainerApp>();

            services.AddSingleton(provider =>
            {
                var configurationService = provider.GetRequiredService<ConfigurationService>();
                var keys = configurationService.GetApiKeys();
                return new CustomVisionService(keys);
            });                                    
        }
    }
}
