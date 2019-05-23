
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineCamera.Core
{
    public interface IUpdator<T>
    {
        void Start(T parameters, CancellationTokenSource source);
        void Stop();
    }

    /// <summary>
    /// Задача постоянного обновления
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractUpdator<T> : IUpdator<T>, IDisposable
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

        protected CancellationToken token => source.Token;
        protected CancellationTokenSource source;

        public AbstractUpdator(IAppLogger log)
        {
            this.log = log;
        }

        public virtual void Start(T parameters, CancellationTokenSource source)
        {
            this.source = source;
            this.task = new Task(async () => {
                log.Information($"{Name} is starting ...");
                await UpdateAsync(parameters);
                log.Information($"{Name} is complete.");
                OnComplete();
            }, source.Token);
            task.Start();
        }


        protected async Task<bool> SleepTrueIfCanceled(int ms)
        {
            try
            {
                await Task.Delay(ms, token);
                return false;
            }
            catch (OperationCanceledException)
            {
                return true;
            }
        }

        public virtual void Stop()
        {
            try
            {
                source?.Cancel();
            }
            catch(AggregateException e)
            {
                log.Error(e.Message, e);
            }
        }

        public void Dispose()
        {
            Stop();
        }

        public override string ToString() => Name;
    }
}
