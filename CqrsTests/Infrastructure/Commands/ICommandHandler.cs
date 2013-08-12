using System.Threading.Tasks;

namespace CqrsTests.Infrastructure.Commands
{
    public interface ICommandHandler {}
    public interface ICommandHandler<T> : ICommandHandler
       where T : ICommand
    {
        Task Handle(T command);
    }
}