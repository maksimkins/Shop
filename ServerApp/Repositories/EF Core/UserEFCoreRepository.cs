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
    public User IsRegistered(User user)
    {
        return dbcontext.Users.First(u => u.Login == user.Login && u.Password == user.Password);
    }

    public User GetById(int id)
    {
        return dbcontext.Users.FirstOrDefault(u => u.Id == id)
            ?? throw new ArgumentNullException("There is no user with such ID");
    }

    public async void Post(User user)
    {
        await dbcontext.Users.AddAsync(user);
        await dbcontext.SaveChangesAsync();
    }

    public async void Update(User user)
    {
        dbcontext.Users.Update(user);
        await dbcontext.SaveChangesAsync();
    }
}
