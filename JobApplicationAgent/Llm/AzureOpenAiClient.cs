using Azure;
using Azure.AI.OpenAI;
using JobApplicationAgent.Interfaces;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;

namespace JobApplicationAgent.Llm;

public class AzureOpenAiClient : ILlmClient
{
    private readonly OpenAIClient _client;
    private readonly string _deployment;

    public AzureOpenAiClient(IConfiguration config)
    {
        var endpoint = new Uri(config["AzureOpenAI:Endpoint"]!);
        var apiKey = new AzureKeyCredential(config["AzureOpenAI:ApiKey"]!);
        _deployment = config["AzureOpenAI:Deployment"]!;

        _client = new AzureOpenAIClient(endpoint, apiKey);
    }

    public async Task<string> GenerateAsync(string prompt)
    {
        var chatClient = _client.GetChatClient(_deployment);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage("You are a helpful assistant."),
            new UserChatMessage("Write a one-sentence summary of agentic AI.")
        };

        var response = await chatClient.CompleteChatAsync(messages, new ChatCompletionOptions
        {
            Temperature = 0.7f
        });

        var text = response.Value.Content.Last().Text;

        return text;
    }
}