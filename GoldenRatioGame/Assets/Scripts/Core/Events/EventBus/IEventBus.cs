using TDS.Handlers;

namespace TDS.Events
{
    public interface IEventBus: IPublisher, ISubscriber
    {
        
    }

    public interface IPublisher
    {
        void Publish<TType>(TType obj);
    }

    public interface ISubscriber
    {
        void Subscribe<TType>(IHandler<TType> handler);
        void Unsubscribe<TType>(IHandler<TType> handler);
    }
}