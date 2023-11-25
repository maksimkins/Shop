using ClientApp.Utilities.Mediator.Interfaces;
using ClientApp.Utilities.Mediator.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Utilities.Mediator;

public class Messenger : IMessenger
{
    private Dictionary<Type, List<Action<IMessage>>> dict;

    public Messenger()
    {
        dict = new Dictionary<Type, List<Action<IMessage>>>();
    }

    public void Subscribe<T>(Action<IMessage> action) where T : IMessage
    {
        var type = typeof(T);

        if (dict.ContainsKey(type) == false)
            dict.Add(type, new List<Action<IMessage>>());

        dict[type].Add(action);
    }

    public void Send<T>(T message) where T : IMessage
    {
        var type = typeof(T);

        if (dict.ContainsKey(type) == false)
            return;

        foreach (var action in dict[type])
        {
            action.Invoke(message);
        }
    }
}
