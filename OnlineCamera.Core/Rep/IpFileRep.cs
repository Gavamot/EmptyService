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

    public class IpFileRep : IVideoRegReqvestSettingsReader
    {
        readonly string fileName;
        public IpFileRep(string fileName)
        {
            this.fileName = fileName;
        }

        /// <exception cref="FileNotFoundException">unable to read file</exception>
        /// <exception cref="FormatException">file content has bad format</exception>
        public VideoRegReqvestSettings[] ReadAll()
        {
            string[] allRows;
            try
            {
                allRows = File.ReadAllLines(fileName);
            }
            catch (Exception e)
            {
                throw new FileNotFoundException($"File {fileName} not exist or permission denied");
            }

            try
            {
                return allRows
                    .Where(x => !x.StartsWith("//"))
                    .Select(x=>x.Trim())
                    .Select(x => {
                        var v = x.Split(' ');
                        if (v.Length > 2) // Нужно конвертировать
                        {
                            var sizeStr = v[1].Split('x');
                            return new VideoRegReqvestSettings
                            {
                                Ip = v[0],
                                Size = new Size
                                {
                                    Width = int.Parse(sizeStr[0]),
                                    Height = int.Parse(sizeStr[1])
                                },
                                Quality = int.Parse(v[2]),
                                IsNeedConvert = true
                            };
                        }
                        else // Без конвертации
                        {
                            return new VideoRegReqvestSettings
                            {
                                Ip = v[0],
                                IsNeedConvert = false
                            };
                        }                   
                    })
                    .ToArray();
            }
            catch (Exception e)
            {
                throw new FormatException($"Each rows in file {fileName} must have format like [122.1.1.1:33 800x600]");
            }
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
