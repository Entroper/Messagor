using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Messagor;

public class MessageLoggerProvider(IServiceProvider services) : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, MessageLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);

    public ILogger CreateLogger(string categoryName)
    {
        var logger = new MessageLogger(services.GetRequiredService<MessageService>());
        _loggers.GetOrAdd(categoryName, logger);

        return logger;
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}
