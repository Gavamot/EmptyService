using OnlineCamera.Core.Value;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OnlineCamera.Core
{
    public interface IVideoRegReqvestSettingsReader
    {
        VideoRegReqvestSettings[] ReadAll();
    }

    class FileReader : IVideoRegReqvestSettingsReader
    {
        readonly string fileName;
        public FileReader(string fileName)
        {
            this.fileName = fileName;
        }

       
        public VideoRegReqvestSettings[] ReadAll()
        {
            var regex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b:\d{1:5} \d{3:4}x\d{3:4}");
            var res = File.ReadAllLines(fileName)
                .Select(x => regex.Match(x).Value)
                .Select(x => {
                    var v = x.Split(' ');
                    var sizeStr = v[1].Split('x');
                    return new VideoRegReqvestSettings
                    {
                        Ip = v[0],
                        Size = new Size
                        {
                            Width = int.Parse(sizeStr[0]),
                            Height = int.Parse(sizeStr[1])
                        }
                    };
                })
                .ToArray();
            return res;
        }
    }
    
    public class IpRep : IIpRep
    {
        private readonly VideoRegReqvestSettings[] data;

        public IpRep(IVideoRegReqvestSettingsReader ipReader)
        {
            this.data = ipReader.ReadAll();
        }

        public VideoRegReqvestSettings[] GetAll()
        {
            return data;
        }
    }
}
