using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Common;
namespace Hw4.Exercise1;

public sealed class PrimesApplication
{
    private readonly IFileSystemProvider _fileSystemProvider;

    public PrimesApplication(IFileSystemProvider fileSystemProvider)
    {
        _fileSystemProvider = fileSystemProvider;
    }

    /// <summary>
    /// Runs application.
    /// </summary>
    public ReturnCode Run()
    {
        Stopwatch sw = new();
        sw.Start();
        if (_fileSystemProvider.Exists("app.settings"))
        {
            string result = "{" + Environment.NewLine;
            result += @" ""success"": true," + Environment.NewLine;
            int minValue = 0;
            int maxValue = 0;
            StreamReader sr = new StreamReader(_fileSystemProvider.Read("app.settings"));
            var s = sr.ReadToEnd();
            string str = "";
            MatchCollection match = Regex.Matches(s, @"primesFrom=(-?\d+)");
            if (match.Count != 0)
            {
                foreach (Match m in match)
                {
                    str += m.Groups[1];
                }
                minValue = int.Parse(str);
            }
            else
            {
                ErrorCorruptedHandler(_fileSystemProvider, sw);
                return ReturnCode.Error;
            }
            str = "";
            match = Regex.Matches(s, @"primesTo=(-?\d+)");
            if (match.Count != 0)
            {
                foreach (Match m in match)
                {
                    str += m.Groups[1];
                }
                maxValue = int.Parse(str);
            }
            else
            {
                ErrorCorruptedHandler(_fileSystemProvider, sw);
                return ReturnCode.Error;
            }
            string range = string.Format(@" ""range"": ""{0}-{1}"",", minValue, maxValue);
            string primes = @" ""primes"": [ ";

            if (minValue < 0)
                minValue = 0;

            if (maxValue < 0)
                maxValue = 0;

            for (int i = minValue; i <= maxValue; i++)
            {
                if (IsPrime(i))
                {
                    primes += i;
                    primes += ",";
                }
            }
            primes = primes.Remove(primes.Length - 1, 1);
            primes += "]";
            string erorr = @" ""error"": null,";
            result += erorr + Environment.NewLine;
            result += range + Environment.NewLine;
            sw.Stop();
            string duration = string.Format(@" ""duration"": ""{0}"",", sw.Elapsed);
            result += duration + Environment.NewLine;
            result += primes + Environment.NewLine;
            result += "}";
            _fileSystemProvider.Write("results.json", new MemoryStream(Encoding.UTF8.GetBytes(result)));
            return ReturnCode.Success;
        }
        else
        {
            ErrorHandler(_fileSystemProvider, sw);
            return ReturnCode.Error;
        }
    }

    public bool IsPrime(int a)
    {
        if (a is 1 or 0)
        {
            return false;
        }
        for (int i = 2; i <= Math.Sqrt(a); i++)
        {
            if (a % i == 0)
            {
                return false;
            }
        }
        return true;
    }

    public static void ErrorHandler(IFileSystemProvider fsp, Stopwatch sw)
    {
        string result = "{" + Environment.NewLine;
        result += @" ""success"": false," + Environment.NewLine;
        result += @" ""error"": ""app.settings is missing"",";
        sw.Stop();
        string duration = string.Format(@" ""duration"": ""{0}"",", sw.Elapsed);
        result += duration + Environment.NewLine;
        result += @" ""primes"": null" + Environment.NewLine;
        result += "}";
        fsp.Write("results.json", new MemoryStream(Encoding.UTF8.GetBytes(result)));
    }

    public static void ErrorCorruptedHandler(IFileSystemProvider fsp, Stopwatch sw)
    {
        string result = "{" + Environment.NewLine;
        result += @" ""success"": false," + Environment.NewLine;
        result += @" ""error"": ""app.settings is corrupted"",";
        sw.Stop();
        string duration = string.Format(@" ""duration"": ""{0}"",", sw.Elapsed);
        result += duration + Environment.NewLine;
        result += @" ""primes"": null" + Environment.NewLine;
        result += "}";
        fsp.Write("results.json", new MemoryStream(Encoding.UTF8.GetBytes(result)));
    }
}
