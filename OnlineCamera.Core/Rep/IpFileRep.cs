using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OnlineCamera.Rep
{
    public interface IFileReader
    {
        string[] ReadAllStringsFromFile(string fileName);
    }

    class FileReader : IFileReader
    {
        public string[] ReadAllStringsFromFile(string fileName)
        {
            return File.ReadAllLines(fileName);
        }
    }

    public class IpFileRep : IIpRep
    {
        readonly string fileName;
        readonly IFileReader fileReader;
        public IpFileRep(string fileName)
        {
            this.fileName = fileName;
        }

        public IpFileRep(string fileName, IFileReader fileReader) : this(fileName)
        {
            this.fileReader = fileReader;

        }

        public string[] GetAll()
        {
            var regex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
            return File.ReadAllLines(fileName)
                .Select(x => regex.Match(x).Value)
                .ToArray();
        }
    }
}
