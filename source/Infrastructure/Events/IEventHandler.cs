using System.Threading.Tasks;

namespace CqrsTests.Infrastructure.Events
{
    public interface IEventHandler {}
    public interface IEventHandler<T> : IEventHandler
    {
        Task Handle(T e);
    }
}