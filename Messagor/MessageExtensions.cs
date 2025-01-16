using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Messagor;

public static class MessageExtensions
{
    public static ILoggingBuilder AddMessageLogger(this ILoggingBuilder builder)
    {
        builder.Services.AddSingleton<MessageService>();
        builder.Services.AddSingleton<ILoggerProvider, MessageLoggerProvider>();

        return builder;
    }
}
