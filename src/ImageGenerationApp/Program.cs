using Azure;
using Azure.AI.OpenAI;
using ImageGenerationApp.Utils;

// Create header
ConsoleHelper.CreateHeader();

// Get Host
string host =
    ConsoleHelper.SelectFromOptions(
        [Statics.OpenAIKey, Statics.AzureOpenAIKey]);

// OpenAI Client
OpenAIClient? client = null;
string deploymentName = "dall-e-3";

switch (host)
{
    case Statics.OpenAIKey:

        // Get OpenAI Key
        string openAIKey =
            ConsoleHelper.GetString(
                $"Please insert your [yellow]{Statics.OpenAIKey}[/] API key:");

        // Create OpenAI client
        client = new(openAIKey);

        break;

    case Statics.AzureOpenAIKey:

        // Get Endpoint
        string endpoint =
            ConsoleHelper.GetUrl(
                "Please insert your [yellow]Azure OpenAI endpoint[/]:");

        // Get Azure OpenAI Key
        string azureOpenAIKey =
            ConsoleHelper.GetString(
                $"Please insert your [yellow]{Statics.AzureOpenAIKey}[/] API key:");

        // Get Deployment name
        deploymentName =
            ConsoleHelper.GetString(
                $"Please insert the name of your [yellow]DALL-E3 model[/]:");

        // Create OpenAI client
        client =
            new(new Uri(endpoint), new AzureKeyCredential(azureOpenAIKey));

        break;
}

if (client == null)
{
    return;
}

while (true)
{
    string? prompt =
        ConsoleHelper.GetString(
            "Please insert the [yellow]prompt for your image[/]:");

    ImageGenerationOptions imageGenerationOptions = new()
    {
        DeploymentName = deploymentName,
        Prompt = prompt,
        Quality = ImageGenerationQuality.Standard,
        Style = ImageGenerationStyle.Vivid
    };

    ConsoleHelper.WriteString("Generating image...");

    Response<ImageGenerations> result =
        await client.GetImageGenerationsAsync(imageGenerationOptions);

    var dataResult = result.Value.Data[0];

    ConsoleHelper.WriteString($"Your image was created.{Environment.NewLine}{Environment.NewLine}" +
        $"You will find it here: {dataResult?.Url}{Environment.NewLine}{Environment.NewLine}" +
        $"The model used the following prompt: [yellow]{dataResult?.RevisedPrompt}[/]{Environment.NewLine}{Environment.NewLine}" +
        "Press any key to continue.");

    Console.ReadKey();
}