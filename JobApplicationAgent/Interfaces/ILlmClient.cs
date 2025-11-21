namespace JobApplicationAgent.Interfaces
{
    public interface ILlmClient
    {
        Task<string> GenerateAsync(string prompt);
    }
}