using SharedProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Repositories.Base;

public interface IUserRepository
{
    public User IsRegistered(User user);
    public void Post(User user);
    public void Update(User user);
    public User GetById(int id);

}
