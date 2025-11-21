using System.Text;
using System.Text.Json;
using JobApplicationAgent.Interfaces;

namespace JobApplicationAgent.LLM
{
    public class OpenAiClient(string apiKey) : ILlmClient
    {
        private readonly HttpClient _httpClient = new();

        public async Task<string> GenerateAsync(string prompt)
        {
            var requestBody = new
            {
                model = "gpt-4o-mini", // or "gpt-4.1" depending on your subscription
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful assistant." },
                    new { role = "user", content = prompt }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);

            return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? string.Empty;
        }
    }
}