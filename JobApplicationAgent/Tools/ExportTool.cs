using JobApplicationAgent.Interfaces;
using QuestPDF.Fluent;

namespace JobApplicationAgent.Tools
{
    public class ExportTool : ITool
    {
        public string Name => ToolsCollection.ExportTool;

        public string Description => "Exports CV and cover letter to PDF.";

        public async Task<string> ExecuteAsync(string input)
        {
            // input: outputsDir|cvContent|coverLetterContent
            var parts = input.Split('|', 3);
            var outputsDir = parts[0];
            var cvContent = parts[1];
            var clContent = parts[2];

            Directory.CreateDirectory(outputsDir);

            var cvPdfPath = Path.Combine(outputsDir, "cv.pdf");
            var clPdfPath = Path.Combine(outputsDir, "cl.pdf");

            // Render CV PDF
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Content().Text(cvContent);
                });
            }).GeneratePdf(cvPdfPath);

            // Render Cover Letter PDF
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Content().Text(clContent);
                });
            }).GeneratePdf(clPdfPath);

            return await Task.FromResult($"Exported PDFs: {cvPdfPath}, {clPdfPath}");
        }
    }
}