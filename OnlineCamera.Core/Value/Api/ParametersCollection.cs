using MoreLinq;
using OnlineCamera.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCamera.Core.Value.Api
{

    public class ParametersCollection
    {
        readonly Dictionary<string, string> col = new Dictionary<string, string>();
        readonly ApiDataTransformer dataTransformer = new ApiDataTransformer();

        public ParametersCollection()
        {
            
        }

        public Dictionary<string, string> ToDictionary() => col.ToDictionary();

        public ParametersCollection(ParametersCollectionKeyValue[] parameters)
        {
            foreach(var p in parameters)
            {
                Add(p);
            }
        }


        public void Add(ParametersCollectionKeyValue keyValue)
        {
            Add(keyValue.Key, keyValue.Value);
        }

        public void Add(string key, object value)
        {
            string v;
            if (value is DateTime)
            {
                v = dataTransformer.ToString((DateTime)value);
            }
            else
            {
                v = value.ToString();
            }
            col.Add(key, v);
        }
    }
}
