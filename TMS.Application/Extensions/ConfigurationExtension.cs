using Microsoft.Extensions.Configuration;

namespace TMS.Application.Extensions
{
    public static class ConfigurationExtension
    {
        extension(IConfiguration configuration)
        {
            public T GetRequiredValue<T>(string key)
            {
                if (string.IsNullOrWhiteSpace(key))
                    throw new ArgumentException("Configuration key cannot be null or empty.", nameof(key));

                T? value = configuration.GetValue<T>(key);

                if (EqualityComparer<T>.Default.Equals(value, default) || value is null)
                    throw new InvalidOperationException($"Missing required configuration value: '{key}'.");

                return value;
            }
        }
    }
}