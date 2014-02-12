using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Commands
{
    public class CommandBus
    {
        private IDictionary<Type, ICommandHandler> _handlers = new Dictionary<Type, ICommandHandler>();

        public void Register(ICommandHandler commandHandler)
        {
            var genericHandler = typeof(ICommandHandler<>);
            var supportedCommandTypes = commandHandler.GetType()
                                                      .GetInterfaces()
                                                      .Where(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == genericHandler)
                                                      .Select(iface => iface.GetGenericArguments()[0])
                                                      .ToList();

            if (_handlers.Keys.Any(supportedCommandTypes.Contains))
            {
                throw new ArgumentException("The command handled by the received handler already has a registered handler.");
            }

            // Register this handler for each of the handled types.
            foreach (var commandType in supportedCommandTypes)
            {
                _handlers.Add(commandType, commandHandler);
            }
        }

        public async Task SendAsync(ICommand command)
        {
            ICommandHandler commandHandler;

            if (_handlers.TryGetValue(command.GetType(), out commandHandler))
            {
                await ((dynamic) commandHandler).Handle((dynamic)command);
            }
        }
    }
}