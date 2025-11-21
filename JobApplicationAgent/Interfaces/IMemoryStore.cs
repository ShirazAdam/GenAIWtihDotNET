namespace JobApplicationAgent.Interfaces
{
    public interface IMemoryStore
    {
        void Save(string key, string value);
        string Get(string key);
    }
}
