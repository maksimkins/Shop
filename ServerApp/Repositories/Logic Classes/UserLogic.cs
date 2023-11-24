﻿using ServerApp.Repositories.Base;
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

    public bool IsRegistered(User user)
    {
        return userRepository.IsRegistered(user);
    }
    public void Post(User user)
    {
        userRepository.Post(user);
    }

    public void Update(User user)
    {
        user.CreationalDate = DateTime.Now;
        userRepository.Update(user);
    }
}
