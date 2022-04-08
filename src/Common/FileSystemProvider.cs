using System.IO;
using System.Text;

namespace Common;

public class FileSystemProvider : IFileSystemProvider
{
    public bool Exists(string filename)
    {
        try
        {
            return File.Exists(filename);
        }
        catch
        {
            return false;
        }
    }

    public Stream Read(string filename)
    {
        return File.OpenRead(filename);
    }

    public void Write(string filename, Stream stream)
    {
        if (stream.Length == 0)
            return;

        stream.Seek(0, SeekOrigin.Begin);

        using var sw = File.AppendText(filename);
        stream.CopyTo(sw.BaseStream);
        stream.Flush();

        /* MemoryStream ms = new MemoryStream();
         stream.CopyTo(ms);
         ms.Position = 0;
         StreamWriter sw = new StreamWriter(filename);
         sw.Write(Encoding.UTF8.GetString(ms.ToArray(), 0, ms.ToArray().Length));*/

    }
}
