[![.NET](https://github.com/ShirazAdam/GenAIWtihDotNET/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ShirazAdam/GenAIWtihDotNET/actions/workflows/dotnet.yml)

# GenAI with .NET

A console application demonstrating how to run generative AI models locally using Microsoft's ONNX Runtime GenAI with .NET.

## Overview

This project showcases a simple yet powerful implementation of running the Phi-3 model locally using ONNX Runtime. The application provides an interactive console interface for conversational AI interactions with streaming token generation and performance metrics.

## Features

- 🚀 **Local AI Execution** - Run AI models completely offline on your machine
- 💬 **Interactive Chat Interface** - Continuous conversation loop for multiple queries
- ⚡ **Streaming Responses** - Real-time token-by-token output generation
- 📊 **Performance Metrics** - Track tokens per second and generation time
- 🔧 **Configurable Parameters** - Adjustable max length and search options

## Prerequisites

- .NET 10.0 or higher
- Microsoft.ML.OnnxRuntimeGenAI NuGet package
- Phi-3 ONNX model files (quantised INT4 model recommended)
- Docker Desktop (optional, for containerised deployment)

## Installation

1. Clone the repository:
```powershell
git clone https://github.com/yourusername/GenAIWithDotNET.git
cd GenAIWithDotNET/GenAI
```

2. Install the required NuGet package:
```powershell
dotnet add package Microsoft.ML.OnnxRuntimeGenAI
```

3. Download the Phi-3 ONNX model:
   - Visit [Microsoft's Phi-4 model page](https://huggingface.co/microsoft/Phi-4-mini-instruct-onnx)
   - Download the quantised model (e.g., `gpu-int4-rtn-block-32`)
   - Extract to a local directory

## Configuration

Update the `modelPath` constant in `Program.cs` to point to your model directory:

```csharp
const string modelPath = @"D:\onnx\gpu\gpu-int4-rtn-block-32";
```

## Usage

### Running Locally

1. Run the application:
```powershell
dotnet run
```

2. Enter your prompt when requested:
```
Prompt:
What is machine learning?
```

3. The model will generate a streaming response with performance metrics:
```
Machine learning is a subset of artificial intelligence...

Streaming Tokens: 156 Time: 2.45 Tokens per second: 63.67

Next prompt:
```

4. Continue the conversation or press Enter without text to exit.

### Running with Docker

The project includes a Dockerfile for containerised deployment using Windows containers.

1. Build the Docker image:
```powershell
docker build -t genai-dotnet .
```

2. Run the container with the model mounted as a volume:
```powershell
docker run -it -v D:\onnx:C:\onnx genai-dotnet
```

**Note**: The Dockerfile uses Windows Nano Server base images. Ensure your Docker Desktop is configured for Windows containers. You'll need to mount your model directory as a volume and update the model path accordingly in the container.

**Docker Configuration**:
- Base image: `mcr.microsoft.com/dotnet/runtime:10.0-nanoserver-ltsc2022`
- Build image: `mcr.microsoft.com/dotnet/sdk:10.0-nanoserver-ltsc2022`
- Multi-stage build for optimised image size

## How It Works

The application follows these steps:

1. **Model Loading** - Loads the ONNX model and initialises the tokeniser
2. **Configuration** - Sets generation parameters (max length: 2048 tokens)
3. **Input Processing** - Encodes user prompts with chat template tags
4. **Token Generation** - Generates tokens one at a time for streaming output
5. **Performance Tracking** - Measures generation speed and token count

## Model Parameters

- **Max Length**: 2048 tokens
- **past_present_share_buffer**: Disabled for compatibility
- **Chat Template**: Uses `<|user|>` and `<|assistant|>` tags for conversation structure

## Performance

Performance varies based on:
- CPU/GPU capabilities
- Model quantisation level (INT4, INT8, FP16, FP32)
- System memory available
- Prompt complexity

Typical performance with INT4 quantisation: 40-100 tokens/second on modern CPUs.

## Technologies Used

- **C# / .NET 10.0** - Core application framework
- **ONNX Runtime GenAI** - AI model inference engine
- **Phi-3 Model** - Microsoft's small language model optimised for efficiency
- **Docker** - Container platform for Windows Nano Server deployment

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Licence

This project is licensed under the MIT Licence - see the LICENCE file for details.

## Acknowledgements

- Microsoft for the ONNX Runtime GenAI library
- Microsoft for the Phi-3 model family

## Resources

- [ONNX Runtime GenAI Documentation](https://onnxruntime.ai/docs/genai/)
- [Phi-3 Model Information](https://azure.microsoft.com/en-us/products/phi-3)
- [Microsoft ML .NET](https://dotnet.microsoft.com/apps/machinelearning-ai/ml-dotnet)

## Troubleshooting

**Issue**: Model fails to load
- Verify the model path is correct
- Ensure all model files are present in the directory
- Check that you have sufficient disk space

**Issue**: Slow performance
- Consider using a more quantised model (INT4 vs FP16)
- Ensure no other resource-intensive applications are running
- Check CPU/RAM usage during generation

**Issue**: Out of memory errors
- Reduce the `max_length` parameter
- Use a more aggressively quantised model
- Close other applications to free up RAM

**Issue**: Docker container fails to start
- Ensure Docker Desktop is set to Windows containers mode
- Verify the model volume mount path is correct
- Check that Windows container features are enabled in Docker settings

**Issue**: Cannot access model files in Docker
- Ensure the volume is mounted correctly with `-v` flag
- Update the model path in the code to match the container path
- Verify file permissions on the host machine
