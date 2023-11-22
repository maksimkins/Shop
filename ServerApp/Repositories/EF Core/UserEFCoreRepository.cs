using ServerApp.Repositories.Base;
using ServerApp.Repositories.EF_Core.DbContext;
using SharedProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Repositories.EF_Core;

public class UserEFCoreRepository : IUserRepository
{
    private readonly ShopDbContext dbcontext;

    public UserEFCoreRepository()
    {
        dbcontext = new ShopDbContext();
    }
    public bool IsRegistered(User user)
    {
        return dbcontext.Users.Any(u => u.Login == user.Login && u.Password == user.Password);
    }

    public async void Post(User user)
    {
        await dbcontext.Users.AddAsync(user);
        await dbcontext.SaveChangesAsync();
    }
}
