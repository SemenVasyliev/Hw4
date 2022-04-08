namespace Common;

public interface IFileSystemProvider
{
    bool Exists(string filename);
    Stream Read(string filename);
    void Write(string filename, Stream stream);
}
