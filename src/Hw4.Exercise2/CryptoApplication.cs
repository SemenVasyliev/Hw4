using System.Text;
using Common;
namespace Hw4.Exercise2;

public class CryptoApplication
{
    private readonly IFileSystemProvider _fileSystemProvider;

    public CryptoApplication(IFileSystemProvider fileSystemProvider)
    {
        _fileSystemProvider = fileSystemProvider;
    }

    /// <summary>
    /// Runs crypto application.
    /// </summary>
    /// <param name="args">Command line arguments</param>
    /// <returns>
    /// Returns <see cref="ReturnCode.Success"/> in case of successful encrypt/decrypt process.
    /// Returns <see cref="ReturnCode.InvalidArgs"/> in case of invalid <paramref name="args"/>.
    /// </returns>
    public ReturnCode Run(string[] args)
    {
        if (args.Length == 0 || args is null)
        {
            return ReturnCode.InvalidArgs;
        }
        string path = "input.txt.enc";
        string filename = args[0];
        int offset = 3;
        string mode = "enc";

        if (args.Length == 2)
        {
            if (int.TryParse(args[1], out offset))
            {
                offset = int.Parse(args[1]);
            }
            if (args[1].ToLower() == "dec")
            {
                mode = "dec";
            }
        }

        if (args.Length == 3)
        {
            if (int.TryParse(args[1], out offset))
            {
                offset = int.Parse(args[1]);
            }
            if (args[1].ToLower() == "dec")
            {
                mode = "dec";
            }
            if (int.TryParse(args[2], out offset))
            {
                offset = int.Parse(args[2]);
            }
            if (args[2].ToLower() == "dec")
            {
                mode = "dec";
            }
        }

        if (mode == "dec")
        {
            path = "input.txt.dec";
        }

        if (!_fileSystemProvider.Exists(filename))
        {
            return ReturnCode.Error;
        }

        StreamReader sr = new StreamReader(_fileSystemProvider.Read(filename));
        var text = sr.ReadToEnd();
        if (text is null)
        {
            return ReturnCode.Error;
        }

        string result = "";
        switch (mode)
        {
            case "enc":
                result = CodeEncryptOrDecrypt(text, offset);
                break;
            case "dec":
                offset = 3; // for default decrypting
                result = CodeEncryptOrDecrypt(text, -offset);
                break;
        }
        _fileSystemProvider.Write(path, new MemoryStream(Encoding.UTF8.GetBytes(result)));
        return ReturnCode.Success;
    }

    public static string CodeEncryptOrDecrypt(string text, int k)
    {
        string alfabet = "abcdefghijklmnopqrstuvwxyz";
        alfabet += "0123456789";
        alfabet += alfabet.ToUpper();
        var letterCount = alfabet.Length;
        var result = "";
        for (int i = 0; i < text.Length; i++)
        {
            var c = text[i];
            var index = alfabet.IndexOf(c);
            if (index < 0)
            {
                result += c.ToString();
            }
            else
            {
                var resultIndex = (letterCount + index + k) % letterCount;
                result += alfabet[resultIndex];
            }
        }

        return result;
    }
}
