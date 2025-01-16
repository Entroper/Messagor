using Microsoft.Extensions.Logging;

namespace Messagor;

public class MessageService
{
    public class Message
    {
		public LogLevel Level { get; init; } = LogLevel.Information;
		public DateTime When { get; } = DateTime.Now;
        public required string Content { get; init; }
    }

    public const int MaxQueueLength = 50;

    private event Func<Message, Task>? MessagePublished;

    private readonly Queue<Message> _messages = new Queue<Message>();
    private readonly SemaphoreSlim _queueLock = new SemaphoreSlim(1);

    public async Task PublishMessage(string messageContent, LogLevel level = LogLevel.Information)
    {
        await _queueLock.WaitAsync();
        try
        {
            var message = new Message { Content = messageContent };
            _messages.Enqueue(message);
            if (_messages.Count > MaxQueueLength)
            {
                _messages.Dequeue();
            }
            
            MessagePublished?.Invoke(message);
        }
        finally
        {
            _queueLock.Release();
        }
    }

    public async Task<List<Message>> SubscribeAndFetchHistory(Func<Message, Task> callback)
    {
        await _queueLock.WaitAsync();
        try
        {
            MessagePublished += callback;
            return _messages.ToList();
        }
        finally
        {
            _queueLock.Release();
        }
    }

    public void Unsubscribe(Func<Message, Task> callback)
    {
        MessagePublished -= callback;
    }
}
