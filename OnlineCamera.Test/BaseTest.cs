using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCamera.Test
{
    [TestFixture]
    public class BaseTest
    {
        public void AreEqual(object actual, object res)
        {
            Assert.AreEqual(JsonConvert.SerializeObject(actual), JsonConvert.SerializeObject(res));
        }
    }
}
