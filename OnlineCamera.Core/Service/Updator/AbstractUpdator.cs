
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
        public abstract string Name { get; }
        protected Task task { get; set; }
        protected abstract void UpdateAsync(T parameters);

        protected CancellationToken token => source.Token;
        protected CancellationTokenSource source;

        public virtual void Start(T parameters, CancellationTokenSource source)
        {
            this.source = source;
            this.task = new Task(()=> { UpdateAsync(parameters); }, source.Token);
            task.Start();
        }


        protected async Task<bool> SleepTrueIfCanceled(int ms)
        {
            try
            {
                await Task.Delay(ms, token);
                return false;
            }
            catch (OperationCanceledException e)
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

            }
        }

        public void Dispose()
        {
            Stop();
        }

        public override string ToString() => Name;
    }
}
