using FakeItEasy;
using NUnit.Framework;
using OnlineCamera.Service;
using OnlineCamera.Test;
using OnlineCamera.Value;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class CameraCacheTest : BaseTest
    {
        private VideoRegCache cache;
        IDateService timeService;

        private CameraResponce GetResponce(DateTime date)
        {
            return new CameraResponce()
            {
                Image = new byte[1] { 2 },
                TimeStamp = date
            };
        }

        [SetUp]
        public void Setup()
        {
            var arr = new Dictionary<Camera, CameraResponce>();
            arr.Add(new Camera("191.1.1.1", 1), GetResponce(new DateTime(2000, 1, 1, 1, 2, 0)));
            arr.Add(new Camera("191.1.1.1", 2), GetResponce(new DateTime(2000, 1, 1, 1, 1, 0)));
            arr.Add(new Camera("191.1.1.2", 1), GetResponce(new DateTime(2000, 1, 1, 1, 3, 0)));

            timeService = A.Fake<IDateService>();
            A.CallTo(() => timeService.GetCurrentDateTime()).Returns(new DateTime(2000, 1, 1, 1, 4, 0));

            cache = new VideoRegCache(timeService);
            cache.SetCasche(arr);
        }

        [Test]
        public void Empty()
        {
            var actual = new VideoReg[0];
            var res = new VideoRegCache(timeService).GetCameras();
            AreEqual(actual, res);
        }

        [Test]
        public void Simople()
        {
            var actual = new VideoReg[]
            {
                new VideoReg{
                    Ip = "191.1.1.1",
                    BrigadeCode = 1,
                    VideoRegSerial = "N1",
                    Version = "1.0",
                    IveSerial = "IVE-1",
                    Cameras = new CameraInfo[]
                    {
                        new CameraInfo { Number = 1, OldSecond = 120  },
                        new CameraInfo { Number = 2, OldSecond = 180 }
                    }
                },
                 new VideoReg{
                    Ip = "191.1.1.2",
                    BrigadeCode = 1,
                    VideoRegSerial = "N2",
                    Version = "1.0",
                    IveSerial = "IVE-1",
                    Cameras = new CameraInfo[]
                    {
                        new CameraInfo { Number = 1, OldSecond = 60  },
                    }
                },
            };
            var res = cache.GetCameras();
            AreEqual(actual, res);
        }
    }
}