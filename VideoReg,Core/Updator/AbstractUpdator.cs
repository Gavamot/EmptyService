
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VideoReg.Core
{
    public interface IUpdator<T>
    {
        void Start(T parameters);
    }

    /// <summary>
    /// Задача постоянного обновления
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractUpdator<T> : IUpdator<T>
    {

        protected virtual void OnComplete()
        {
            // Вызывается после завершение работы обновлятора
            // Используется для определения события OnUpdateComplete в классах потомках
        }

        public abstract string Name { get; }
        protected Task task { get; set; }
        protected abstract Task UpdateAsync(T parameters);

        protected readonly IAppLogger log;

        public AbstractUpdator(IAppLogger log)
        {
            this.log = log;
        }

        public virtual void Start(T parameters)
        {
            this.task = new Task(async () => {
                log.Information($"{Name} is starting ...");
                await UpdateAsync(parameters);
                log.Information($"{Name} is complete.");
                OnComplete();
            });
            task.Start();
        }

        public override string ToString() => Name;
    }
}
