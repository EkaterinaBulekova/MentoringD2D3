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

        private readonly WorkerSettings _config;
        private readonly ILogger<Configurator> _logger;
        private readonly Func<Exception, string> logExceptionText = (ex) => $"Exception - {ex.Message} - {ex.InnerException?.Message}";

        public Configurator(IOptions<WorkerSettings> config, ILogger<Configurator> logger)
        {
            try
            {
                _logger = logger;
                if (config == null && config.Value == null) 
                    throw new ConfigurationNullExceplion(ConfigurationEmpty);
                _config = config.Value;
                if (IsValid())
                    _logger.LogDebug(ConfigurationLoaded);
                else
                    throw new ConfigurationNotValidExceplion(ConfigurationEmpty);
            }
            catch (Exception ex)
            {
                _logger.LogError(logExceptionText(ex));
                throw new CannotCreateConfiguratorException(CannotCreateConfiguration, ex);
            }
        }

        public Directories Directories
        {
            get
            {
                return _config.Directories;
            }
        }

        public FileProcessRule ProcessRule
        {
            get
            {
                return _config.FileProcessRule;
            }
        }
        
        public bool IsValid() 
        {
            return IsDirectoriesValid() && IsProcessRuleValid();
        }

        private bool IsDirectoriesValid()
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(Directories);
            if (!Validator.TryValidateObject(Directories, context, results, true))
            {
                foreach (var error in results)
                {
                    _logger.LogError(error.ErrorMessage);
                }
                return false;
            }
            return true;
        }

        private bool IsProcessRuleValid()
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(ProcessRule);
            if (!Validator.TryValidateObject(ProcessRule, context, results, true))
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
