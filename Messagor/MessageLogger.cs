using Microsoft.Extensions.Logging;

namespace Messagor;

public class MessageLogger(MessageService messageService) : ILogger
{
    private readonly MessageService _messageService = messageService;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }
        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }

        var message = formatter(state, exception);
        var _ = _messageService.PublishMessage(message, logLevel);
    }
}
