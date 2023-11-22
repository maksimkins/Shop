using SharedProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Repositories.Base;

public interface IUserRepository
{
    public bool IsRegistered(User user);
    public void Post(User user);
}
