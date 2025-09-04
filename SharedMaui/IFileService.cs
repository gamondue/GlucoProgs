namespace SharedMaui
{
    internal interface IFileService
    {
        Task<string> PickFileAsync();
        Task SaveFileAsync(string filename, byte[] data);
        Task<byte[]> LoadFileAsync(string filename);
    }
}
