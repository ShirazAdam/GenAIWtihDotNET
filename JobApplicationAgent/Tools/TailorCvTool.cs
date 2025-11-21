using JobApplicationAgent.Interfaces;

namespace JobApplicationAgent.Tools
{
    public class TailorCvTool(ILlmClient llm) : ITool
    {
        public string Name => ToolsCollection.TailorCvTool;

        public string Description => "Tailors CV to job requirements.";

        public Task<string> ExecuteAsync(string input)
        {
            // In practice: call LLM with CV + JD context
            //const string summaryPrompt = "Rewrite the CV so that it demonstrates the strengths which align with the role";
            //var cvSummary =  llm.GenerateAsync(summaryPrompt);
            //return Task.FromResult($"Tailored CV based on: {input}");

            return llm.GenerateAsync($"Tailor CV based on: {input}");
        }
    }
}