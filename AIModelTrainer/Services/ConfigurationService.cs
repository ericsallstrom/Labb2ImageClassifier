using AIModelTrainer.MushroomCustomVision;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIModelTrainer.Services
{
    public class ConfigurationService
    {
        public Keys GetApiKeys()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<Program>();

            var config = builder.Build();

            var keys = new Keys();
            config.GetSection("CustomVision").Bind(keys);

            return keys;
        }
    }
}
