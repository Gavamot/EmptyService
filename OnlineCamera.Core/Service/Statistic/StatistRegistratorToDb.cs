using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCamera.Core.Value.Api;

namespace OnlineCamera.Core.Service.Statistic
{
    class StatistRegistratorToDb : IStatistRegistrator
    {
        public Task RegAsync(string ip, VideoRegResponce responce)
        {
            return new Task(() =>
            {
                // TODO : Доделать статистику
            });
        }
    }
}
