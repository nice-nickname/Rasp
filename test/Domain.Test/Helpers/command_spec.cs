using Incoding.Core.CQRS.Core;
using Incoding.UnitTests.MSpec;

namespace Domain.Test;

class command_spec<TCommand> where TCommand : CommandBase
{
    protected static TCommand command;

    protected static MockMessage<TCommand, object> mockCommand;
}
