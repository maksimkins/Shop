﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProj.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    string Login { get; set; }
    string Password { get; set; }
}
