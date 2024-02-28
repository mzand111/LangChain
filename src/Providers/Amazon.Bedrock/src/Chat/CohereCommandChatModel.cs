﻿using System.Diagnostics;
using System.Text.Json.Nodes;
using LangChain.Providers.Amazon.Bedrock.Internal;

// ReSharper disable once CheckNamespace
namespace LangChain.Providers.Amazon.Bedrock;

public abstract class CohereCommandChatModel(
    BedrockProvider provider,
    string id)
    : ChatModel(id)
{
    public override async Task<ChatResponse> GenerateAsync(
        ChatRequest request,
        ChatSettings? settings = null,
        CancellationToken cancellationToken = default)
    {
        request = request ?? throw new ArgumentNullException(nameof(request));

        var watch = Stopwatch.StartNew();
        var prompt = request.Messages.ToSimplePrompt();

        var usedSettings = BedrockChatSettings.Calculate(
            requestSettings: settings,
            modelSettings: Settings,
            providerSettings: provider.ChatSettings);
        var response = await provider.Api.InvokeModelAsync(
            Id,
            new JsonObject
            {
                ["prompt"] = prompt,
                ["max_tokens"] = usedSettings.MaxTokens!.Value,
                ["temperature"] = usedSettings.Temperature!.Value,
                ["p"] = usedSettings.TopP!.Value,
                ["k"] = usedSettings.TopK!.Value,
            },
            cancellationToken).ConfigureAwait(false);

        var generatedText = response?["generations"]?[0]?["text"]?.GetValue<string>() ?? string.Empty;

        var result = request.Messages.ToList();
        result.Add(generatedText.AsAiMessage());

        var usage = Usage.Empty with
        {
            Time = watch.Elapsed,
        };
        AddUsage(usage);
        provider.AddUsage(usage);

        return new ChatResponse
        {
            Messages = result,
            UsedSettings = usedSettings,
            Usage = usage,
        };
    }
}