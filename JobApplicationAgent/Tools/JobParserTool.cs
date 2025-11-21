using JobApplicationAgent.Interfaces;

namespace JobApplicationAgent.Tools
{
    public class JobParserTool(ILlmClient llm) : ITool
    {
        public string Name => ToolsCollection.JobParserTool;

        public string Description => "Parses job descriptions.";

        public Task<string> ExecuteAsync(string input)
        {
            // Simplified: In real use, call LLM to extract structured info
            //return Task.FromResult($"Parsed job description: {input.Substring(0, Math.Min(50, input.Length))}...");

            return llm.GenerateAsync($"Parse job description for {input}");
        }
    }
}
