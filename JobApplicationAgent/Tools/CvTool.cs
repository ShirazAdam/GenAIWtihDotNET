using JobApplicationAgent.Interfaces;
using Xceed.Words.NET;

namespace JobApplicationAgent.Tools
{
    public class CvTool : ITool
    {
        public string Name => ToolsCollection.CvTool;

        public string Description => "Loads CV from a DOCX file.";

        public async Task<string> ExecuteAsync(string input)
        {
            if (!input.StartsWith("load:")) return await Task.FromResult("Invalid command");

            var path = input.Replace("load:", "");
            using var doc = DocX.Load(path);

            return doc.Text; // returns plain text from the docx
        }
    }
}