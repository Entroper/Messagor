using Microsoft.AspNetCore.Components;

namespace MessagR;

public partial class MessagesComponent : IDisposable
{
	protected Stack<MessageService.Message> _messages = new Stack<MessageService.Message>();

	[Parameter]
	public RenderFragment? ChildContent { get; set; }

	public int Count => _messages.Count;

    protected override async Task OnInitializedAsync()
    {
		var messages = await MessageService.SubscribeAndFetchHistory(MessagePublished);
        foreach (var message in messages)
        {
            _messages.Push(message);
        }
    }

    public void Dispose()
    {
        MessageService.Unsubscribe(MessagePublished);
    }

    private async Task MessagePublished(MessageService.Message message)
	{
		_messages.Push(message);
        await InvokeAsync(StateHasChanged);
    }
}
