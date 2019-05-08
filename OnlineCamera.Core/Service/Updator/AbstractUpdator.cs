
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineCamera.Core
{
    public abstract class AbstractUpdator : IUpdator, IDisposable
    {
        public abstract string Name { get; }
        protected ILogger log { get; set; }
        protected Task task { get; set; }
        protected abstract void UpdateAsync();
        public AbstractUpdator(ILogger log)
        {
            this.log = log;
        }

        protected CancellationTokenSource source;
        public virtual void Start(CancellationTokenSource source)
        {
            this.source = source;
            this.task = new Task(UpdateAsync, source.Token)
                .ContinueWith((t) => log.Info($"{Name} execution is completed"));
            log.Info($"{ Name } service is starting ...");
            task.Start();
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
