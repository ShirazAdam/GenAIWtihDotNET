using JobApplicationAgent.Interfaces;

namespace JobApplicationAgent
{
    public class Agent(ILlmClient llm, List<ITool> tools, IMemoryStore memory)
    {
        public async Task RunAsync(string goal, string masterCvPath, string jobDescPath, string outputsDir)
        {
            Console.WriteLine($"Goal: {goal}");

            // Load CV from DOCX
            var cvTool = tools.Find(t => t.Name == ToolsCollection.CvTool)!;
            var masterCv = await cvTool.ExecuteAsync($"load:{masterCvPath}");

            // Load job description
            var jobDesc = await File.ReadAllTextAsync(jobDescPath);

            // Parse JD
            var jobParser = tools.Find(t => t.Name == ToolsCollection.JobParserTool)!;
            var jobJson = await jobParser.ExecuteAsync(jobDesc);
            memory.Save("jobJson", jobJson);

            // Tailor CV
            var tailor = tools.Find(t => t.Name == ToolsCollection.TailorCvTool)!;
            var tailoredCv = await tailor.ExecuteAsync($"{masterCv}\n\n---\n\n{jobJson}");
            memory.Save("tailoredCv", tailoredCv);

            // CV Summary for cover letter
            var summaryPrompt = $"Summarise the tailored CV in 400 words with role-aligned highlights:\n{tailoredCv}";
            var cvSummary = await llm.GenerateAsync(summaryPrompt);
            memory.Save("cvSummary", cvSummary);

            // Cover Letter
            var coverLetterTool = tools.Find(t => t.Name == ToolsCollection.CoverLetterTool)!;
            var coverLetter = await coverLetterTool.ExecuteAsync(jobJson + "\n\n---\n\n" + tailoredCv);
            memory.Save("coverLetter", coverLetter);

            // Export PDFs
            var export = tools.Find(t => t.Name == ToolsCollection.ExportTool)!;

            var exportResult = await export.ExecuteAsync($"{outputsDir}|{tailoredCv}|{coverLetter}");

            Console.WriteLine(exportResult);
        }

    }
}