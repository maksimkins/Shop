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
    public bool IsRegistered(string login, string password)
    {
        return dbcontext.Users.Any(u => u.Login == login && u.Password == password);
    }

    public async void Register(User user)
    {
        await dbcontext.Users.AddAsync(user);
        await dbcontext.SaveChangesAsync();
    }
}
