// See https://aka.ms/new-console-template for more information

using Microsoft.ML.OnnxRuntimeGenAI;

// The absolute path to the folder where the Phi-4 model is stored (folder to the '.onnx' file)
// using https://huggingface.co/microsoft/Phi-4-mini-instruct-onnx

const string modelPath = @"D:\phi4mini\gpu\gpu-int4-rtn-block-32";
using var config = new Config(modelPath);
config.ClearProviders();

#if DEBUG_CUDA
config.SetProviderOption("cuda", "enable_cuda_graph", "0");
#endif

using var model = new Model(config);
var tokenizer = new Tokenizer(model);

const string initialPrompt = "Answer the following question as clearly and concisely as possible, providing any relevant information and examples.";
Console.WriteLine(initialPrompt);

var generatorParams = new GeneratorParams(model);
generatorParams.SetSearchOption("max_length", 2048);
generatorParams.SetSearchOption("past_present_share_buffer", false);

using var tokenizerStream = tokenizer.CreateStream();
using var generator = new Generator(model, generatorParams);

Console.WriteLine("Prompt:");
var prompt = Console.ReadLine();

do
{
    var sequences = tokenizer.Encode($"<|user|>{prompt}<|end|><|assistant|>");

    generator.AppendTokenSequences(sequences);

    var watch = System.Diagnostics.Stopwatch.StartNew();

    while (!generator.IsDone())
    {
        generator.GenerateNextToken();
        Console.Write(tokenizerStream.Decode(generator.GetSequence(0)[^1]));
    }

    watch.Stop();

    Console.WriteLine();

    var runTimeInSeconds = watch.Elapsed.TotalSeconds;
    var outputSequence = generator.GetSequence(0);
    var totalTokens = outputSequence.Length;

    Console.WriteLine(
        $"Streaming Tokens: {totalTokens} Time: {runTimeInSeconds:0.00} Tokens per second: {totalTokens / runTimeInSeconds:0.00}");

    Console.WriteLine();
    Console.WriteLine("Next prompt:");

    prompt = Console.ReadLine();

} while (!string.IsNullOrWhiteSpace(prompt));
