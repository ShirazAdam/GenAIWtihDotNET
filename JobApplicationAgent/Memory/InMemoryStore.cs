using JobApplicationAgent.Interfaces;

namespace JobApplicationAgent.Memory
{
    public class InMemoryStore : IMemoryStore
    {
        private readonly Dictionary<string, string> _store = [];

        public void Save(string key, string value) => _store[key] = value;

        public string Get(string key) => _store.TryGetValue(key, out var value) ? value : string.Empty;
    }
}