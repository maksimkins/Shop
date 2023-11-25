using ClientApp.Utilities.Mediator.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Utilities.Mediator.Interfaces;


public interface IMessenger
{
    void Subscribe<T>(Action<IMessage> action) where T : IMessage;

    void Send<T>(T message) where T : IMessage;
}
