using JobApplicationAgent.Interfaces;

namespace JobApplicationAgent.Tools
{
    public class CoverLetterTool(ILlmClient llm) : ITool
    {
        public string Name => ToolsCollection.CoverLetterTool;

        public string Description => "Generates cover letters.";

        public Task<string> ExecuteAsync(string input)
        {
            //return Task.FromResult($"Generated cover letter for: {input}");

            return llm.GenerateAsync($"Generated cover letter for: {input}");
        }
    }
}
