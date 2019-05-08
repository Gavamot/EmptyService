using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCamera.Core.Config.VideoRegUpdatorConfig
{
    public class VideoRegUpdatorConfig
    {
        /// <summary>
        /// Частота запросов на наличие камер
        /// </summary>
        public readonly int GetVideoRegInfoIntervalMs;
        /// <summary>
        /// Желаемый fps скорее всего будет меньше чем хочется
        /// </summary>
        public readonly double Fps;
        /// <summary>
        /// Максимальное время ожидания ответа от камеры
        /// </summary>
        public readonly int DelayMs;
        /// <summary>
        /// Количество попыток взять видео с камеры после чего следует прекратить делать бесполезную работу
        /// </summary>
        public readonly int CountOfTrys;
    }

}
