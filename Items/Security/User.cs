﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Items.Security
{
    public class User
    {
        public int Id { set; get; }
        public string Login { set; get; }
        public string Password { set; get; }
        public int RoleId { set; get; }
    }
}
