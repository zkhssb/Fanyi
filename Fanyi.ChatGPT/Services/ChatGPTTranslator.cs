using System.ClientModel;
using Fanyi.Abstractions;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;

namespace Fanyi.ChatGPT.Services;

public class ChatGPTTranslator(IOptions<ChatGPTTranslatorOptions> options, ITranslatorOptions translatorOptions)
    : ITranslator
{
    private readonly ChatClient _client = new(
        options.Value.Model ?? "gpt-3.5-turbo",
        new ApiKeyCredential(!string.IsNullOrWhiteSpace(options.Value.ApiKey)
            ? options.Value.ApiKey
            : Guid.Empty.ToString()), // ApiKey如果是string.Empty会报错,so 如果没有apiKey 也需要一个瞎写的ApiKey
        new OpenAIClientOptions
        {
            Endpoint = new Uri(options.Value.UseProxy ? options.Value.ProxyUrl! : "https://api.openai.com/v1")
        }
    );

    public async Task<string> TranslateAsync(string text)
    {
        var to = IsAscii(text) ? "中文" : "英语";
        if (translatorOptions.GetTo() is SpecificLanguage targetLanguage)
        {
            to = targetLanguage.Name;
        }

        List<ChatMessage> messages = [];
        var prompt = "你是一位精通各种语言的专业翻译，接下来我会发送一些文本字段，请你帮我翻译为: {to}"
            .Replace("{to}", to);
        messages.AddRange(
            [
                ChatMessage.CreateUserMessage(prompt),
                ChatMessage.CreateAssistantMessage("OK"),
                ChatMessage.CreateUserMessage(text)
            ]
        );

        // 直接使用CompleteChatAsync(messages)可能会漏掉消息,不知道为什么,在此使用流式聊天
        AsyncCollectionResult<StreamingChatCompletionUpdate> completionUpdates =
            _client.CompleteChatStreamingAsync(messages);

        List<string> result = [];
        await foreach (var completionUpdate in completionUpdates)
        {
            if (completionUpdate.ContentUpdate.Count > 0)
            {
                result.Add(completionUpdate.ContentUpdate[0].Text);
            }
        }

        return string.Join(string.Empty, result) ??
               throw new Exception("ChatGPT 翻译接口响应无效");
    }

    public bool IsEnable()
        => options.Value.Enable;

    public string GetName()
        => "ChatGPT";

    private static bool IsAscii(string text) => text.All(c => c <= 127);
}