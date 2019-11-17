using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CofigurationService
{
    public class Configurator : IConfigurator
    {
        private const string ConfigurationEmpty = "Configuration is empty";
        private const string ConfigurationLoaded = "Configuration loaded";
        private const string CannotCreateConfiguration = "Cannot create configuration";

        private readonly MoverSettings _config;
        private readonly ILogger<Configurator> _logger;
        private readonly Func<Exception, string> logExceptionText = (ex) => $"Exception - {ex.Message} - {ex.InnerException?.Message}";

        public Configurator(IOptions<MoverSettings> config, ILogger<Configurator> logger)
        {
            try
            {
                _logger = logger;
                if (config == null && config.Value == null) 
                    throw new ConfigurationNullExceplion(ConfigurationEmpty);
                _config = config.Value;
                if (IsValid())
                    _logger.LogInformation(ConfigurationLoaded);
                else
                    throw new ConfigurationNotValidExceplion(ConfigurationEmpty);
            }
            catch (Exception ex)
            {
                _logger.LogError(logExceptionText(ex));
                throw new CannotCreateConfiguratorException(CannotCreateConfiguration, ex);
            }
        }

        public string TargetDirectory => _config.TargetDirectory;

        public string ServiceBusConnectionString => _config.ServiceBusConnectionString;

        public string StorageAccountConnectionString => _config.StorageAccountConnectionString;

        public int WaitTimeout =>_config.WaitTimeout;

        public int HoursAttachmentLive => _config.HoursAttachmentLive;

        public string QueueName => _config.QueueName;

        public string TopicName => _config.TopicName;

        public string CilentName => _config.ClientName;

        public bool IsValid() 
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(_config);
            if (!Validator.TryValidateObject(_config, context, results, true))
            {
                foreach (var error in results)
                {
                    _logger.LogError(error.ErrorMessage);
                }
                return false;
            }
            return true;
        }
    }
}
