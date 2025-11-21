using JobApplicationAgent;
using JobApplicationAgent.Interfaces;
using JobApplicationAgent.Llm;
using JobApplicationAgent.Memory;
using JobApplicationAgent.Tools;
using Microsoft.Extensions.Configuration;

namespace JobApplicationAgent
{
    internal class Program
    {
        private static async Task Main()
        {
            // Load appsettings.json via ConfigurationBuilder
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var llm = new AzureOpenAiClient(config);

            var tools = new List<ITool>
            {
                new CvTool(),
                new JobParserTool(llm),
                new TailorCvTool(llm),
                new CoverLetterTool(llm),
                new ExportTool()
            };

            var memory = new InMemoryStore();

            var agent = new Agent(llm, tools, memory);

            var masterCvPath = config["Paths:MasterCv"]!;
            var jobDescPath = config["Paths:JobDescription"]!;
            var outputsDir = config["Paths:OutputsDir"]!;

            // Kick off with explicit goal
            string goal = $"Tailor CV and cover letter for the job in {jobDescPath} using base CV {masterCvPath} and write outputs to {outputsDir}.";

            await agent.RunAsync(goal, masterCvPath!, jobDescPath!, outputsDir!);
        }
    }
}
