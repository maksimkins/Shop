using ServerApp.Repositories.Base;
using SharedProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Repositories.Logic_Classes;

public class UserLogic
{
    private readonly IUserRepository userRepository;

    public UserLogic(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public User IsRegistered(User user)
    {
        return userRepository.IsRegistered(user);
    }
    public void Post(User user)
    {
        user.CreationalDate = DateTime.Now;
        userRepository.Post(user);
    }

    public User GetById(int id)
    {
        return userRepository.GetById(id);
    }

    public void Update(User user)
    {
        user.CreationalDate = DateTime.Now;
        userRepository.Update(user);
    }
}
