namespace MochiBTS.Core.Primitives.Utilities.Event
{
    public interface ISubscribable
    {
        public void Subscribe(IListener listener);
        public void Unsubscribe(IListener listener);
    }
}