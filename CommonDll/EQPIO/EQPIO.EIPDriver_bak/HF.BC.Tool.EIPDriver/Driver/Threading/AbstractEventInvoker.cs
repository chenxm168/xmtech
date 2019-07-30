
namespace HF.BC.Tool.EIPDriver.Driver.Threading
{
    using HF.BC.Tool.EIPDriver.Driver.Data;
    using HF.BC.Tool.EIPDriver.Driver.EventHandler;
    using System;

    public abstract class AbstractEventInvoker : AbstractThread
    {
        protected BlockingQueue<EIPEvent> eventQueue = new BlockingQueue<EIPEvent>();

        protected AbstractEventInvoker()
        {
        }

        public void FireReceivedEvent(object sender, object data)
        {
            EIPEvent item = new EIPEvent
            {
                Sender = sender,
                Data = data
            };
            this.eventQueue.Enqueue(item);
        }

        public override void Terminate()
        {
            base.Terminate();
            this.eventQueue.Clear();
        }
    }
}
